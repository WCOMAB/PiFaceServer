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

    internal interface IPiFaceDriver
    {
        Int32 WiringPiSetupPiFace();
        void PullUpDnControl(Int32 pin, Int32 pud);
        void DigitalWrite(Int32 pin, Int32 value);
        Int32 DigitalRead(Int32 pin);
    }
}