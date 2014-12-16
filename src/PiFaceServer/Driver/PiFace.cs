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
namespace PiFaceServer.Driver
{
    using System;
    using System.Linq;
    using System.Threading;
    using Helper;
    using Timer = System.Timers.Timer;

    public static class PiFace
    {
        private static int[] _outputs = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private static int[] _inputs = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private static int[] _pullUpDown = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private static int _initResult = -1;
        private static Action<int, int> _inputChanged;
        
        public static readonly bool IsRunningMono = Type.GetType ("Mono.Runtime") != null;

        private static readonly IPiFaceDriver PiFaceDriver = IsRunningMono
                                                         ? new PiFaceDriverNativeMethods()
                                                         : new PiFaceDriverEmulator() as IPiFaceDriver;
        private static readonly Timer InputReader = GetInputReader();

        public static Action<int, int> InputChanged
        {
            get { return _inputChanged; }
            set
            {
                _inputChanged = value;
                if (value != null && !InputReader.Enabled)
                    InputReader.Start();
                else if (value==null && InputReader.Enabled)
                    InputReader.Stop();
            }
        }

      


        private static Timer GetInputReader()
        {
            var inputReader =  new Timer
                             {
                                 Interval = 50,
                                 AutoReset = true
                             };
            inputReader.Elapsed += (s, e) => InputReaderElapsed();
            return inputReader;
        }

        private static void InputReaderElapsed()
        {
            var currentInputs = new int[8];
            _inputs.CopyTo(currentInputs, 0);
            var newInputs = Enumerable.Range(
                0,
                8
                ).Select(
                    pin => PiFaceDriver.DigitalRead(pin)
                ).ToArray();
            if (newInputs.SequenceEqual(currentInputs))
                return;

            Interlocked.Exchange(ref _inputs, newInputs);

            var inputChanged = _inputChanged;
            if (inputChanged == null) return;

            for (var pin = 0; pin < 8; pin++)
            {
                if (currentInputs[pin] != newInputs[pin])
                {
                    inputChanged(pin, newInputs[pin]);
                }
            }
        }


        public static Int32 WiringPiSetupPiFace()
        {
            var initResult = -1;
            try
            {
                PiFaceDriver.TryAndLog(
                    driver => initResult = driver.WiringPiSetupPiFace()
                    );

                if (initResult == -1)
                    return initResult;

                Interlocked.Exchange(
                    ref _outputs,
                    new[] {0, 0, 0, 0, 0, 0, 0, 0}
                    );
                Interlocked.Exchange(
                    ref _pullUpDown,
                    new[] {0, 0, 0, 0, 0, 0, 0, 0}
                    );
                Interlocked.Exchange(
                    ref _inputs,
                    Enumerable.Range(
                        0,
                        8
                        ).Select(
                            pin => PiFaceDriver.DigitalRead(pin)
                        ).ToArray()
                    );
                return initResult;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -1;
            }
            finally
            {
                Interlocked.Exchange(ref _initResult, initResult);
            }
        }

        public static Int32 PullUpDnControl(Int32 pin, Int32 pud)
        {
            if (_initResult == -1) return -1;
            try
            {
                PiFaceDriver.PullUpDnControl(
                    pin,
                    _pullUpDown[pin] = pud
                    );
                return _pullUpDown[pin];
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -1;
            }
        }
        public static Int32 DigitalWrite(Int32 pin, Int32 value)
        {
            if (_initResult == -1) return -1;
            try
            {
                PiFaceDriver.DigitalWrite(
                    pin,
                    _outputs[pin] = value
                    );
                return _outputs[pin];
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -1;
            }
        }

        public static Int32 DigitalRead(Int32 pin)
        {
            if (_initResult == -1) return -1;
            try
            {
                return _inputs[pin] = PiFaceDriver.DigitalRead(pin);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return -1;
            }
        }
    }
}
