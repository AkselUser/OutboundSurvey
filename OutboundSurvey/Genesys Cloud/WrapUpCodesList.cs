using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboundSurvey
{
    internal class WrapUpCodesList
    {
        internal static List<string> GetWrapUpCodes()
        {
            int? pageCount = API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["wrapUpDataTableId"]).PageCount;
            var listOfWrapUpCodes = new List<string>();
            for (int i = 0; i < pageCount; i++)
            {
                API.architectApi.GetFlowsDatatableRows(System.Configuration.ConfigurationManager.AppSettings["wrapUpDataTableId"], i).Entities.ForEach(x => listOfWrapUpCodes.Add(x.Values.First().ToString()));
            }
            return listOfWrapUpCodes;
        }
    }
}
