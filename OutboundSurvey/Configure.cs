using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;

namespace OutboundSurvey
{
    class Configure
    {
        internal static Channel channel { get; set; }

        internal static void RunConfig()
        {
            channel = new Channel();
            SetEnvironment();
            SetToken();

            channel = API.notificationsApi.PostNotificationsChannels();
            Notification.SetupNotifications(API.notificationsApi, channel);
        }

        internal static void SetToken()
        {
            var clientId = System.Configuration.ConfigurationManager.AppSettings["clientId"];
            var clientSecret = System.Configuration.ConfigurationManager.AppSettings["clientSecret"];
            try
            {
                // Set access token as described in GC documentation
                AuthTokenInfo accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(
                    clientId,
                    clientSecret);
                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = accessTokenInfo.AccessToken;
            
            Log.Logger.Info("New OAuth token retrieved: " + accessTokenInfo.AccessToken);
            }
            catch (Exception ex) { Log.Logger.Info("Set Token error: " + ex); }
        }

        internal static void SetEnvironment()
        {
            // Set environment
            PureCloudRegionHosts region = PureCloudRegionHosts.eu_west_1;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);
            Log.Logger.Info("Environment set: {0}", region);
        }
    }
}
