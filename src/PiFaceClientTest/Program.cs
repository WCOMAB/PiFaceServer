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
namespace PiFaceClientTest
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using ProjectInclude.PiFaceClient;
    using ProjectInclude.PiFaceClient.Source.Common;
    using Command = ProjectInclude.PiFaceServer.Command.Command;

    internal static class Program
    {
        private const int Port = 31415;

        private static void Main(string[] args)
        {
            Thread.Sleep(1500);
            var ipString = args.FirstOrDefault() ?? "127.0.0.1";


            Test(ipString);
        }

        private static async void Test(string ipString)
        {
            var ipAddress = IPAddress.Parse(ipString);
            var remoteEp = new IPEndPoint(ipAddress, Port);

            using (var client = new PiFaceClient(remoteEp))
            {
                client.Connect();

                var initResult = await client.WiringPiSetupPiFaceAsync();
                Log.HighLight(initResult.ToString());

                Array.ForEach(
                    Enumerable.Range(0, 8).ToArray(),
                    async pin =>
                        {
                            var pudResult = await client.PullUpDnControlAsync(pin, PullUpDn.Up);
                            Log.HighLight(pudResult.ToString());
                        }
                    );
                var dwResult = await client.DigitalWriteAsync(0, PinState.High);
                Log.HighLight(dwResult.ToString());

                Thread.Sleep(5000);

                dwResult = await client.DigitalWriteAsync(0, PinState.Low);
                Log.HighLight(dwResult.ToString());

                var rdResult = await client.DigitalReadAsync(0);

                Log.HighLight(rdResult.ToString());

                
                client.BeginMonitorInput(
                    0,
                    callback: client.ControlSaftblandare
                    );

                Console.Read();
            }
        }

        private static void ControlSaftblandare(this PiFaceClient client, Command command)
        {
            client.BeginDigitalWrite(0, command.Value == 1 ? PinState.Low : PinState.High);
        }
    }
}
