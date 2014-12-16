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
namespace PiFaceServer
{
    using System;
    using System.Threading;

    internal static class Program
    {
        private static void Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            PiFaceServer.Start(cancellationTokenSource.Token);
            Console.CancelKeyPress += (s, e) =>
                                          {
                                              e.Cancel = true;
                                              cancellationTokenSource.Cancel(false);
                                          };
        }
    }
}
