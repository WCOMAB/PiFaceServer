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
namespace PiFaceServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Command;
    using Driver;
    using Helper;
    using Source.Common;

    public static class PiFaceServer
    {
        private static readonly ManualResetEvent RequestAcceptedEvent = new ManualResetEvent(false);

        private static readonly ConcurrentDictionary<string, BlockingCollection<Command.Command>> AckQueueDictionary = new ConcurrentDictionary<string, BlockingCollection<Command.Command>>();
        private static readonly ConcurrentDictionary<string, Command.Command[]> MonitorInputDictionary = new ConcurrentDictionary<string, Command.Command[]>();

        private static void Server(this int port, CancellationToken cancellationToken)
        {
            try
            {
                var socket = PiFace.IsRunningMono
                                 ? new Socket(
                                       AddressFamily.InterNetworkV6,
                                       SocketType.Stream,
                                       ProtocolType.Tcp
                                       )
// Dual mode constructor missing in mono (Ipv4 & IpV6)
// test show sofar mono is dual mode by default
// ReSharper disable PossibleNullReferenceException
                                 : (Socket) (
                                                typeof (Socket).GetConstructor(
                                                    new[]
                                                        {
                                                            typeof (SocketType),
                                                            typeof (ProtocolType)
                                                        }
                                                ).Invoke(
// ReSharper restore PossibleNullReferenceException
                                                    new object[]
                                                        {
                                                            SocketType.Stream,
                                                            ProtocolType.Tcp
                                                        }
                                                )
                                            );

                socket.Bind(new IPEndPoint(IPAddress.IPv6Any, port));

                socket.Listen(1024);
                cancellationToken.Register(() => RequestAcceptedEvent.TryAndLog(rae => rae.Set()));
                while (!cancellationToken.IsCancellationRequested)
                {
                    RequestAcceptedEvent.Reset();
                    socket.BeginAccept();
                    RequestAcceptedEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public static bool IsConnected(this Socket socket)
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }


        private static void BeginAccept(this Socket socket)
        {
            socket.TryAndLog(
                s =>
                    {
                        s.BeginAccept(s.AcceptCallback, null);
                        Log.Info("Waiting for connection...");
                    }
                );
        }

        private static void AcceptCallback(this Socket socket, IAsyncResult asyncResult)
        {
            socket.TryAndLog(
                s =>
                    {
                        RequestAcceptedEvent.Set();
                        Log.Info("Incoming request...");
                        var handler = s.EndAccept(asyncResult);
                        Log.Info("{0} connected to {1}", handler.RemoteEndPoint, handler.LocalEndPoint);
                        handler.BeginReceive();
                    }
                );
        }

        private static void BeginReceive(this Socket socket)
        {
            socket.TryAndLog(
                handler =>
                    {
                        var commandState = new CommandState
                                               {
                                                   Handler = handler,
                                                   CommandData = new byte[Command.Command.SizeOf]
                                               };
                        handler.BeginReceive(
                            commandState.CommandData,
                            0,
                            commandState.CommandData.Length,
                            0,
                            commandState.ReceiveCallback,
                            null
                            );
                    }
                );
        }

        public static void ReceiveCallback(this CommandState commandState, IAsyncResult asyncResult)
        {
            commandState.AsyncResult = asyncResult;
            commandState.TryAndLog(
                state =>
                    {
                        SocketError socketError;
                        var readBytes = commandState.Handler.EndReceive(commandState.AsyncResult, out socketError);
                        if (readBytes == 0 && !IsConnected(commandState.Handler))
                        {
                            using (commandState.Handler)
                                return;
                        }
                        var localEndPoint = (IPEndPoint)commandState.Handler.LocalEndPoint;
                        if (socketError == SocketError.Success)
                        {
                            var ipAddress = ((IPEndPoint) commandState.Handler.RemoteEndPoint).Address.ToString();
                            Log.Success(
                                "Receive {0} bytes from {1} connected to {2}",
                                readBytes,
                                ipAddress,
                                localEndPoint
                                );
                            commandState.CommandData.TryAndLog(
                                commandData =>
                                    {
                                        var command = Command.Command.FromArray(commandData);
                                        Log.Info(
                                            command.ToString()
                                            );
                                        var remoteEndPoint = string.Format(
                                            "{0}:{1}",
                                            ipAddress,
                                            localEndPoint.Port
                                            );

                                        if (command.Operation == Operation.Connect)
                                        {
                                            new AckState
                                                {
                                                    Command = command,
                                                    RemoteEndPoint = remoteEndPoint,
                                                    Handler = commandState.Handler,
                                                    Queue = new BlockingCollection<Command.Command>{command},
                                                    AsyncResult = null
                                                }.BeginSend();
                                            return;
                                        }

                                        BlockingCollection<Command.Command> ackQueue;
                                        if (
                                            AckQueueDictionary.TryGetValue(
                                            remoteEndPoint,
                                            out ackQueue
                                            )
                                            )
                                        {
                                            ackQueue.TryAdd(command);
                                        }
                                    }
                                );
                            commandState.Handler.BeginReceive();
                        }
                        else
                        {
                            Log.Error(
                                "Receive error {0} from {1} connected to {2}",
                                socketError,
                                commandState.Handler.RemoteEndPoint,
                                localEndPoint
                                );
                        }
                    }
                );
        }

        private static void BeginSend(this AckState ackState)
        {
            ackState.TryAndLog(
            ack=>
                {
                    AckQueueDictionary.AddOrUpdate(
                        ack.RemoteEndPoint,
                        key => ack.Queue,
                        (key, current) =>
                            {
                                current.CompleteAdding();
                                return ack.Queue;
                            }
                        );
                    while (!ack.Queue.IsCompleted)
                    {
                        var command = ack.Queue.Take();
                        Command.Command ackCommand;
                        try
                        {
                            switch (command.Operation)
                            {
                                case Operation.Connect:
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: 1
                                        );
                                    break;

                                case Operation.WiringPiSetupPiFace:
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: PiFace.WiringPiSetupPiFace()
                                        );
                                    break;

                                case Operation.PullUpDnControl:
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: PiFace.PullUpDnControl(command.Pin, command.Value)
                                        );
                                    break;

                                case Operation.DigitalWrite:
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: PiFace.DigitalWrite(command.Pin, command.Value)
                                        );
                                    break;

                                case Operation.DigitalRead:
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: PiFace.DigitalRead(command.Pin)
                                        );
                                    break;

                                case Operation.MonitorInput:
                                    var monitor  = MonitorInputDictionary.AddOrUpdate(
                                        ackState.RemoteEndPoint,
                                        key =>
                                            {
                                                var value = new Command.Command[8];
                                                value[command.Pin] = command;
                                                return value;
                                            },
                                        (key, value) =>
                                            {
                                                value[command.Pin] = command;
                                                return value;
                                            }
                                        );
                                    var result = monitor[command.Pin];
                                    ackCommand = GetAckCommand(
                                        command,
                                        value: (result.Id == command.Id && command.Value == result.Value)
                                                   ? 1
                                                   : 0
                                        );
                                    if (PiFace.InputChanged == null)
                                        PiFace.InputChanged = MonitorInputChanged;
                                    break;

                                case Operation.MonitorInputChanged:
                                    ackCommand = command;
                                    break;

                                    // ReSharper disable RedundantCaseLabel
                                case Operation.None:
                                default:
                                    // ReSharper restore RedundantCaseLabel
                                    ackCommand = GetAckCommand(
                                        command,
                                        operation: Operation.None,
                                        value: PiFace.DigitalRead(command.Pin)
                                        );
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.LogException();
                            ackCommand = GetAckCommand(
                                command,
                                value: -1
                                );
                        }

                        var ackCommandBytes = ackCommand.ToArray();
                        Log.Info(
                               "Sending {0} bytes from {1} connected to {2}",
                               ackCommandBytes.Length,
                               ack.Handler.LocalEndPoint,
                               ack.Handler.RemoteEndPoint
                               );
                        ackState.Handler.BeginSend(
                            ackCommandBytes,
                            0,
                            ackCommandBytes.Length,
                            0,
                            ackState.SendCallback,
                            null
                            );
                    }
                }
            );
        }

        private static void MonitorInputChanged(int pin, int value)
        {
            MonitorInputDictionary.TryAndLog(
                mid => Array.ForEach(
                    mid.Keys.ToArray(),
                    key => key.TryAndLog(
                        remoteEndpoint =>
                            {
                                Command.Command[] monitoring;
                                BlockingCollection<Command.Command> ackQueueu;
                                if (
                                    !mid.TryGetValue(remoteEndpoint, out monitoring) ||
                                    !AckQueueDictionary.TryGetValue(remoteEndpoint, out ackQueueu) ||
                                    ackQueueu.IsAddingCompleted
                                    )
                                    return;

                                var command = monitoring[pin];
                                if (command.Value == 1)
                                    ackQueueu.Add(
                                        GetAckCommand(
                                            command,
                                            operation:Operation.MonitorInputChanged,
                                            value: value
                                            )
                                        );
                            }
                               )
                           )
                );
        }

        private static Command.Command GetAckCommand(
            Command.Command command,
            byte? target = null,
            long? id = null,
            Operation? operation = null,
            int? pin = null,
            int? value = null
            )
        {
            var ackCommand = new Command.Command
                                     {
                                         Id = id ?? command.Id,
                                         Operation = operation ?? command.Operation,
                                         Pin = pin ?? command.Pin,
                                         Value = value ?? -1,
                                         Target = target ?? command.Target
                                     };
            return ackCommand;
        }

        private static void SendCallback(this AckState ackState, IAsyncResult asyncResult)
        {
            ackState.AsyncResult = asyncResult;
            ackState.TryAndLog(
                ack =>
                    {
                        var bytesSent = ack.Handler.EndSend(ack.AsyncResult);
                        Log.Success(
                               "Sent {0} bytes from {1} connected to {2}",
                               bytesSent,
                               ack.Handler.LocalEndPoint,
                               ack.Handler.RemoteEndPoint
                               );
                    }
                );
        }

        public static void Start(CancellationToken cancellationToken, int port = 31415)
        {
            new Thread(
                () => port.Server(cancellationToken)
                )
                {
                    IsBackground = false,
                    Name = string.Format("PiFace Server on port {0}", port)
                }.Start();
        }
    }
}