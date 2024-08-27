using OutboundSurvey.Genesys_Cloud;
using PureCloudPlatform.Client.V2.Model;
using System.Collections.Specialized;
using System.Configuration;

namespace OutboundSurvey
{
    internal class Contacts
    {

        

        internal static WritableDialerContact NewContact(string phoneNumber, string conversationId)
        {
            WritableDialerContact contact = new WritableDialerContact(Callable: true);
            contact.Data = new Dictionary<string, object>();
            contact.Data.Add("Phone", phoneNumber);
            contact.Data.Add("ConversationIdAttribute", conversationId);
            return contact;
        }





        internal static void ClearContacts()
        {
            Log.Logger.Info("Starting to clear contact lists...");
            int? pageCount = API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["configDataTableId"], showbrief: false).PageCount;
            for (int i = 0; i < pageCount; i++)
            {
                try
                {
                    var contactListEntity = API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["configDataTableId"], pageNumber: i, showbrief: false).Entities;
                    foreach (var entity in contactListEntity)
                    {
                        var contactListId = entity.First(x => x.Key == "ContactListId").Value.ToString();
                        Log.Logger.Info("Getting contactlist Id: " + contactListId);
                        var campaign = API.outboundApi.GetOutboundCampaigns(contactListId: contactListId).Entities.First();
                        Log.Logger.Info("Using campaign: " + campaign.Id);
                        campaign.CampaignStatus = Campaign.CampaignStatusEnum.Off;

                        API.outboundApi.PutOutboundCampaignAsync(campaign.Id, campaign).Wait();
                        Log.Logger.Info($"Turned off outbound campaign: {campaign.Name}");


                        API.outboundApi.PostOutboundContactlistClear(contactListId);
                        Log.Logger.Info("Clearing contact list for culture: \"" + entity.First(x => x.Key == "ContactListId").Value.ToString() + "\"");

                        var updatedCampaign = API.outboundApi.GetOutboundCampaign(campaign.Id);
                        updatedCampaign.CampaignStatus = Campaign.CampaignStatusEnum.On;

                        API.outboundApi.PutOutboundCampaignAsync(campaign.Id, updatedCampaign).Wait();
                        Log.Logger.Info("Outbound campaign turned back on");
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex);
                }
                Log.Logger.Info("Finished clearing contact lists");
            }
        }
    }
}
