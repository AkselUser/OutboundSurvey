using OutboundSurvey.Genesys_Cloud;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;

namespace OutboundSurvey
{
    internal static class Notification
    {
        public static void SetupNotifications(NotificationsApi notificationsApi, Channel channel)
        {
            try
            {
                Log.Logger.Info("Setting up notification");
                var allQueues = AllQueues.GetAllQueues();
                var channelTopicList = new List<ChannelTopic>();
                foreach (var queue in allQueues)
                {
                    channelTopicList.Add(
                        new ChannelTopic()
                        {
                            Id = $"v2.routing.queues.{queue}.conversations"
                        });
                }
                notificationsApi.PutNotificationsChannelSubscriptions(channel.Id, channelTopicList);
                Log.Logger.Info("Set up notification finished. Channel Id:\n " + channel.Id);
            }
            catch (Exception ex)
            {
                Log.Logger.Info(ex);
            }
        }
    }
}