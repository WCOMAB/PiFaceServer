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
namespace PiFaceServer.Command
{
    public enum Operation : byte
    {
        None = 0,
        Connect = 1,
        WiringPiSetupPiFace = 2,
        PullUpDnControl = 3,
        DigitalWrite = 4,
        DigitalRead = 5,
        MonitorInput = 6,
        MonitorInputChanged = 7
    }
}