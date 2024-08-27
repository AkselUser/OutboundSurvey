using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboundSurvey.Genesys_Cloud
{
    internal class AllQueues
    {
        public static List<string> GetAllQueues()
        {
            var queues = new List<string>();
            var routingQueues = API.routingApi.GetRoutingQueues(pageSize: 100);

            for (int i = 1; i <= routingQueues.PageCount; i++)
            {
                var queueEntityListing = API.routingApi.GetRoutingQueues(pageSize: 100, pageNumber: i);
                foreach (var entity in queueEntityListing.Entities)
                {
                    queues.Add(entity.Id);
                }
            }
            return queues;
        }
    }
}
