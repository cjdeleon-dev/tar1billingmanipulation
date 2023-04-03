using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.DisconListAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.DisconListService
{
    public class DisconListService : IDisconListService
    {
        IDisconListAccess idla;

        public DisconListService()
        {
            idla = new DisconListAccess();
        }

        public List<DisconListModel> GetSubForDisconList(int nummonths, string status, string route)
        {
            return idla.GetSubForDisconList(nummonths, status, route);
        }
    }
}