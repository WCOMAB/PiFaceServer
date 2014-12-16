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
namespace PiFaceAPI
{
    using System;
    using System.Net;
    using System.Threading;
    using PiFaceClient;

    // ReSharper disable once InconsistentNaming
    public partial class test : System.Web.UI.Page
    {
        private const int Port = 31415;
        private static readonly Semaphore LockObject = new Semaphore(1,1);
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "text/plain";
                int pin;
                switch (Request["pin"])
                {
                    case "0":
                        pin = 0;
                        break;
                    case "1":
                        pin = 1;
                        break;
                    default:
                        pin = -1;
                        break;
                }
                var reqPinState = Request["pinstate"];
                var pinState =
                    StringComparer.InvariantCultureIgnoreCase.Equals(reqPinState, "High")
                        ? PinState.High
                        : (
                              StringComparer.InvariantCultureIgnoreCase.Equals(reqPinState, "Low")
                                  ? PinState.Low
                                  : PinState.Error
                          );
                int autoOff;
                autoOff = int.TryParse(Request["autooff"], out autoOff)
                              ? autoOff
                              : -1;
                IPAddress ipAddress;
                if (pin < 0 || pinState == PinState.Error || !IPAddress.TryParse(Request["ip"], out ipAddress)) return;
                var remoteEp = new IPEndPoint(ipAddress, Port);
                using (var client = new PiFaceClient.PiFaceClient(remoteEp))
                {
                    try
                    {
                        LockObject.WaitOne();
                        client.Connect();
                        var initResult = await client.WiringPiSetupPiFaceAsync();
                        Response.Write(initResult.ToString());
                        Response.Write("\r\n");

                        var dwResult = await client.DigitalWriteAsync(pin, pinState);
                        Response.Write(dwResult.ToString());
                        Response.Write("\r\n");
                        Response.Flush();
                        Thread.Sleep(500);

                        if (autoOff <= 0 || pinState != PinState.High) return;

                        Thread.Sleep(autoOff);
                        dwResult = await client.DigitalWriteAsync(pin, PinState.Low);
                        Response.Write(dwResult.ToString());
                        Response.Write("\r\n");
                        Response.Flush();
                        Thread.Sleep(500);
                    }
                    finally
                    {
                        LockObject.Release();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}