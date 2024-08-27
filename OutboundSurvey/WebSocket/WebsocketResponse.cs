using PureCloudPlatform.Client.V2.Model;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace OutboundSurvey
{
    internal class WebsocketResponse
    {
        internal static async Task RunWebSocket()
        {
            ClientWebSocket Client = new ClientWebSocket();
            Uri uri = new Uri(Configure.channel.ConnectUri);
            await Client.ConnectAsync(uri, CancellationToken.None);
            using (var client = Client)
            {
                // While loop runs forever
                string rawOutput = "";
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = null;
                //var variable = "";
                //Task.Run(() =>
                //{
                //    Thread.Sleep(86300 * 1000);
                //    variable = "finish";
                //});
                Log.Logger.Info("Websocket started");

                while (true)
                {
                    try
                    {
                        while (client.State == WebSocketState.Open)
                        {
                            do
                            {
                                //if(variable == "finish")
                                //{
                                //    variable = "";
                                //    return;
                                //}
                                result = await client.ReceiveAsync(bytesReceived, CancellationToken.None).ConfigureAwait(false);

                                if (result.Count > 0)
                                {
                                    rawOutput += Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                                }
                                else
                                {
                                    Log.Logger.Info("Over");
                                }
                            } while (!result.EndOfMessage);


                            if (!String.IsNullOrEmpty(rawOutput))
                            {
                                var responseSchema = JsonSerializer.Deserialize<ResponseSchema.Rootobject>(rawOutput);
                                // Check Json is not heartbeat
                                if (responseSchema?.eventBody.participants != null)
                                {
                                    RelevantOutput.EvaluateOutput(responseSchema);
                                }
                            }
                            rawOutput = "";
                        }
                    }
                    catch (Exception ex) { Log.Logger.Info("RunWebsocket issue: " + ex); }
                    Log.Logger.Info("Websocket loop ended");
                    Environment.Exit(0);
                    break;
                }
            }

        }
    }
}