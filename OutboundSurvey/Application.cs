using OutboundSurvey.Genesys_Cloud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutboundSurvey
{
    internal class Application
    {
        public static void StartApplication()
        {
            Log.Logger.Info("Application Started...");

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", @"..\App.config");

            API.InitializeApis();
            Configure.RunConfig();
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(86300 * 1000);
                        Configure.SetEnvironment();
                        Configure.SetToken();
                        Notification.SetupNotifications(API.notificationsApi, Configure.channel);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Info("RunConfig Error: " + ex);
                    }
                }
            });
         
            
            
            // Task for clearing contact lists
            Task.Run(() =>
            {
                while (true)
                {
                    var count = int.Parse(System.Configuration.ConfigurationManager.AppSettings["contactListPeriod"]);
                    if (count == 0)
                    {
                        Thread.Sleep(86300 * 1000);
                    }
                    else
                    {
                        for (var i = 0; i <= count; i++)
                        {
                            Thread.Sleep(86300 * 1000);
                        }
                        Log.Logger.Info("Starting to clear contactList");
                        try
                        {
                            Contacts.ClearContacts();
                        }
                        catch(Exception ex)
                        {
                            Log.Logger.Info("ContactList Error: " + ex);
                        }
                    }
                }
            });

            //WebSocketClient.StartWebsocket().Wait();
            WebsocketResponse.RunWebSocket().Wait();
        }

        public static void StopApplication()
        {

            Log.Logger.Info("Application Stopped");
        }
    }
}