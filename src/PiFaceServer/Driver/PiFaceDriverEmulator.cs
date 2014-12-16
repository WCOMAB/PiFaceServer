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

    class PiFaceDriverEmulator : IPiFaceDriver
    {
        Int32 IPiFaceDriver.WiringPiSetupPiFace()
        {
            return 0;
        }

        void IPiFaceDriver.PullUpDnControl(Int32 pin, Int32 pud)
        {

        }

        void IPiFaceDriver.DigitalWrite(Int32 pin, Int32 value)
        {

        }

        Int32 IPiFaceDriver.DigitalRead(Int32 pin)
        {
            return (DateTime.Now.Second % 10 == 0) ? 1 : 0;
        }
    }
}