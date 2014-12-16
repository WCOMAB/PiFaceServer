// ----------------------------------------------------------------------------------------------
// Copyright (c) WCOM AB.
// ----------------------------------------------------------------------------------------------
// This source code is subject to terms and conditions of the Microsoft Public License. A 
// copy of the license can be found in the LICENSE.md file at the root of this distribution. 
// If you cannot locate the  Microsoft Public License, please send an email to 
// dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
//  by the terms of the Microsoft Public License.
// ----------------------------------------------------------------------------------------------
// You must not remove this notice, or any other, from this software.
// ----------------------------------------------------------------------------------------------
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace PiFaceClient
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Source.Common;
    using PiFaceServer.Command;

    public class PiFaceClient : IDisposable
    {
        private readonly ConcurrentDictionary<long, Action<Command>> _callBackCommands = new ConcurrentDictionary<long, Action<Command>>();
        private readonly IPEndPoint _remoteEndPoint;
        private readonly Socket _ackSocket;
        private readonly Socket _cmdSocket;

        public IPEndPoint RemoteEndpoint
        {
            get { return _remoteEndPoint; }
        }

        public bool IsDisposed { get; private set; }

        public Action<Command> AckRecieved { get; set; }

        public void Connect()
        {
            _ackSocket.Connect(RemoteEndpoint);
            SendCommand(
                _ackSocket,
                new Command
                    {
                        Operation = Operation.Connect
                    }
                );
            Command initAckCommand;
            ReceiveCommand(_ackSocket, out initAckCommand);

            new Thread(
                () =>
                {
                    while (!IsDisposed && IsConnected(_ackSocket))
                    {
                        Command ackCommand;
                        ReceiveCommand(_ackSocket, out ackCommand);
                    }
                }
                ).Start();
            _cmdSocket.Connect(RemoteEndpoint);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            try
            {
                _ackSocket.Dispose();
            }
            catch (Exception ex)
            {
                Log.Exception(ex.ToString());
            }
            try
            {
                _cmdSocket.Dispose();
            }
            catch (Exception ex)
            {
                Log.Exception(ex.ToString());
            }
        }

        private static long _currId = DateTime.UtcNow.Ticks;
        private static long GetNextId()
        {
            return Interlocked.Increment(ref _currId);
        }



        private static void SendCommand(Socket socket, params Command[] commands)
        {
            Array.ForEach(
                commands,
                command =>
                {
                    var commandBytes = command.ToArray();

                    Log.Info(
                        "Sending command {0} ({1})",
                        command,
                        commandBytes.Length
                        );


                    var bytesSent = socket.Send(
                        commandBytes
                        );
                    Log.Success("Sent {0} bytes", bytesSent);
                }
                );
        }

        private void ReceiveCommand(Socket ackSocket, out Command ackCommand)
        {
            var ackBytes = new byte[Command.SizeOf];
            Log.Info("Recieving bytes");
            SocketError errorCode;
            var bytesRecieved = ackSocket.Receive(
                ackBytes,
                0,
                Command.SizeOf,
                0,
                out errorCode
                );
            if (errorCode != SocketError.Success)
            {
                Log.Error("Recieve error {0}", errorCode);
                ackCommand = default(Command);
                return;
            }

            Log.Success("Recieved {0} bytes", bytesRecieved);
            Log.Info(
                "Command from array of {0} bytes ({1})",
                ackBytes.Length,
                string.Concat(
                    ackBytes.Select(x => x.ToString("x2")
                        )
                    )
                );

            ackCommand = Command.FromArray(ackBytes);
            Log.Success("Recived command {0}", ackCommand);

            try
            {
                if (AckRecieved != null)
                    AckRecieved(ackCommand);
            }
            catch (Exception ex)
            {
                Log.Exception(ex.ToString());
            }

            try
            {
                Action<Command> callback;
                if (
                    (
                    (ackCommand.Operation==Operation.MonitorInputChanged || ackCommand.Operation==Operation.MonitorInput)
                    ? _callBackCommands.TryGetValue(ackCommand.Id, out callback)
                    : _callBackCommands.TryRemove(ackCommand.Id, out callback)
                        )&&
                    callback != null
                    )
                {
                    callback(ackCommand);
                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex.ToString());
            }
        }

        private static bool IsConnected(Socket socket)
        {
            return socket.IsBound &&
                   socket.Connected &&
                   !(
                        socket.Poll(1, SelectMode.SelectRead)
                        && socket.Available == 0
                    );
        }



        private static Action<Command> GetCallbackTask(out Task<Command> task)
        {
            var result = default(Command);
            var done = new ManualResetEventSlim();
            Action<Command> callback = command =>
            {
                result = command;
                done.Set();
            };
            task = Task.Run(
                () =>
                    {
                        using (done)
                        {
                            done.Wait(100000);
                            return result;
                        }
                    }
                );
            return callback;
        }

        public PiFaceClient(IPEndPoint remoteEndPoint)
        {
            _remoteEndPoint = remoteEndPoint;
            _ackSocket = new Socket(
                RemoteEndpoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
                );
            _cmdSocket = new Socket(
                RemoteEndpoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
                );
        }

        public Task<Command> WiringPiSetupPiFaceAsync(byte target = 0)
        {
            Task<Command> task;
            BeginWiringPiSetupPiFace(target, GetCallbackTask(out task));
            return task;
        }

        public void BeginWiringPiSetupPiFace(byte target = 0, Action<Command> callback = null)
        {
            var command = GetNewCommand(Operation.WiringPiSetupPiFace, 0, 0, target, callback);
            SendCommand(_cmdSocket, command);
        }

        public Task<Command> PullUpDnControlAsync(int pin, PullUpDn pud, byte target = 0)
        {
            Task<Command> task;
            BeginPullUpDnControl(pin, pud, target, GetCallbackTask(out task));
            return task;
        }

        public void BeginPullUpDnControl(int pin, PullUpDn pud, byte target = 0, Action<Command> callback = null)
        {
            var command = GetNewCommand(Operation.PullUpDnControl, pin, (int)pud, target, callback);
            SendCommand(_cmdSocket, command);
        }

        public Task<Command> DigitalWriteAsync(int pin, PinState pinState, byte target = 0)
        {
            Task<Command> task;
            BeginDigitalWrite(pin, pinState, target, GetCallbackTask(out task));
            return task;
        }

        public void BeginDigitalWrite(int pin, PinState pinState, byte target = 0, Action<Command> callback = null)
        {
            var command = GetNewCommand(Operation.DigitalWrite, pin, (int)pinState, target, callback);
            SendCommand(_cmdSocket, command);
        }

        public Task<Command> DigitalReadAsync(int pin, byte target = 0)
        {
            Task<Command> task;
            BeginDigitalRead(pin, target, GetCallbackTask(out task));
            return task;
        }

        public void BeginDigitalRead(int pin, byte target = 0, Action<Command> callback = null)
        {
            var command = GetNewCommand(Operation.DigitalRead, pin, 0, target, callback);
            SendCommand(_cmdSocket, command);
        }

        public void BeginMonitorInput(int pin, bool? monitor = null, byte target = 0, Action<Command> callback = null)
        {
            var command = GetNewCommand(
                Operation.MonitorInput,
                pin,
                (monitor ?? true) ? 1 : 0,
                target,
                callback
                );
            SendCommand(_cmdSocket, command);
        }

        private Command GetNewCommand(Operation operation, int pin, int value, byte target, Action<Command> callback = null)
        {
            var id = GetNextId();
            if (callback != null)
                _callBackCommands.TryAdd(
                    id,
                    callback
                    );
            return new Command
                               {
                                   Id = id,
                                   Operation = operation,
                                   Target = target,
                                   Pin = pin,
                                   Value = value
                               };
        }
    }
}