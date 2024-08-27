using PureCloudPlatform.Client.V2.Api;

namespace OutboundSurvey
{
    internal class API
    {
        internal static RoutingApi routingApi { get; set; }
        internal static OutboundApi outboundApi { get; set; }
        internal static NotificationsApi notificationsApi { get; set; }
        internal static AnalyticsApi analyticsApi { get; set; }
        internal static ArchitectApi architectApi { get; set; }

        internal static void InitializeApis()
        {
            outboundApi = new OutboundApi();
            notificationsApi = new NotificationsApi();
            analyticsApi = new AnalyticsApi();
            routingApi = new RoutingApi();
            architectApi = new ArchitectApi();
        }
    }
}
