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
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace PiFaceServer.Command
{
    using System;
    using System.Collections.Concurrent;
    using System.Net.Sockets;

    public class AckState
    {
        public Command Command { get; set; }
        public string RemoteEndPoint { get; set; }
        public Socket Handler { get; set; }
        public BlockingCollection<Command> Queue { get; set; }
        public IAsyncResult AsyncResult { get; set; }
    }
}