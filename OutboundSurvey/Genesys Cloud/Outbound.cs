using PureCloudPlatform.Client.V2.Model;

namespace OutboundSurvey
{
    internal class Outbound
    {
        internal static void StartOutbound(ContactInformation contactInformation)
        {
            try
            {
                //contactInformation.campaign.CallerName = contactInformation.conversationId;
                //API.outboundApi.PutOutboundCampaignAsync(contactInformation.campaign.Id, contactInformation.campaign).Wait();
                var contactListId = contactInformation.contactListId;

                var result = API.outboundApi.PostOutboundContactlistContacts(
                    contactListId,
                    new List<WritableDialerContact>(){ contactInformation.contact }
                    );
                Log.Logger.Info($"Contact List for SurveyCulture \"{contactInformation.callSurveyCulture}\" updated:\n\n{{\n\t\"contactListId\": {contactListId}\n\t\"phoneNumber\": {contactInformation.contact.Data["Phone"]}\n}}\n");
            }
            catch (Exception ex)
            {
                Log.Logger.Info(ex);
            }
        }
    }
}
