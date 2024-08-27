using PureCloudPlatform.Client.V2.Model;

namespace OutboundSurvey
{
    internal class ContactInformation
    {
        public string conversationId { get; set; }
        public WritableDialerContact contact { get; set; }
        public string callSurveyCulture { get; set; }
        public string contactListId { get; set; }
    }
}
