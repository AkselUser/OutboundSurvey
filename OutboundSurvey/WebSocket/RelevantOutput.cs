using OutboundSurvey.Genesys_Cloud;
using System.Collections.Specialized;
using System.Configuration;

namespace OutboundSurvey
{
    internal class RelevantOutput
    {
        internal static void EvaluateOutput(ResponseSchema.Rootobject responseSchema)
        {
            List<ContactInformation> contactInformationList = new List<ContactInformation>();
            List<string> quarantinedConversations = new List<string>();
            var participants = responseSchema.eventBody.participants;
            try
            {
                if (participants.Where(p => p.purpose == "agent").Any() && participants.Where(p => p.purpose == "customer").Any())
                {
                    var customer = participants.Where(p => p.purpose == "customer").First();
                    var callSurveyCulture = customer.attributes.CallSurveyCulture;
                    if (customer.attributes != null && callSurveyCulture != null)
                    {
                        var conversationId = responseSchema.eventBody.id;
                        var agent = participants.Where(p => p.purpose == "agent")?.First();

                        var configTable = ConfigDataTableDict.GetDataTableDict(callSurveyCulture);
                        if (customer.calls.First().state == "terminated" &&
                            !quarantinedConversations.Contains(conversationId) &&
                            agent.wrapup != null &&
                            customer.calls.First().disconnectType != "endpoint" &&
                            !PhoneNumberQuarantined.IsPhoneNumberQuarantined(customer.address) &&
                            !WrapUpCodesList.GetWrapUpCodes().Contains(agent.wrapup.code) && 
                            configTable["ExcludedWrapupCode_TableId"].ToString() != agent.wrapup.code
                            )
                        {
                            var contactListId = configTable["ContactListId"].ToString();
                            ContactInformation newContact = new ContactInformation()
                            {
                                contactListId = contactListId,
                                callSurveyCulture = callSurveyCulture,
                                conversationId = conversationId,
                                contact = Contacts.NewContact(customer.address, conversationId)
                            };

                            Task.Run(() =>
                            {
                                quarantinedConversations.Add(conversationId);
                                Log.Logger.Info("Sending Contact: " + newContact.contact.Data["Phone"] + " to Outbound Campaign ..");
                                Outbound.StartOutbound(newContact);
                                Thread.Sleep(10000);
                                quarantinedConversations.Remove(conversationId);
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Logger.Info(ex); }
        }
    }
}
    