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
    using System.Runtime.InteropServices;

    class PiFaceDriverNativeMethods : IPiFaceDriver
    {
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupPiFace")]
        static extern Int32 wiringPiSetupPiFace();

        [DllImport("libwiringPi.so", EntryPoint = "pullUpDnControlPiFace")]
        static extern void pullUpDnControl(Int32 pin, Int32 pud);

        [DllImport("libwiringPi.so", EntryPoint = "digitalWritePiFace")]
        static extern void digitalWrite(Int32 pin, Int32 value);

        [DllImport("libwiringPi.so", EntryPoint = "digitalReadPiFace")]
        static extern Int32 digitalRead(Int32 pin);

        Int32 IPiFaceDriver.WiringPiSetupPiFace()
        {
            return wiringPiSetupPiFace();
        }

        void IPiFaceDriver.PullUpDnControl(Int32 pin, Int32 pud)
        {
            pullUpDnControl(pin, pud);
        }

        void IPiFaceDriver.DigitalWrite(Int32 pin, Int32 value)
        {
            digitalWrite(pin, value);
        }

        Int32 IPiFaceDriver.DigitalRead(Int32 pin)
        {
            return digitalRead(pin);
        }
    }
}