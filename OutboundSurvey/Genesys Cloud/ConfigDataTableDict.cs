using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboundSurvey.Genesys_Cloud
{
    internal class ConfigDataTableDict
    {
        // Gets config from data table in Genesys Cloud
        internal static Dictionary<string, object> GetDataTableDict(string callSurveySurveyCulture)
        {
            int? pageCount = API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["configDataTableId"], showbrief: false).PageCount;
            var entity = new Dictionary<string, object>();
            for (int i = 0; i < pageCount; i++)
            {
                try
                {
                    entity = API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["configDataTableId"], pageNumber: i, showbrief: false).Entities.First(x => x["key"].ToString() == callSurveySurveyCulture);
                } catch(Exception ex)
                {
                    Log.Logger.Error(ex);
                }
            }
            return entity;
        }
    }
}