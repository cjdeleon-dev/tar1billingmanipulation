using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.DisconListService
{
    public interface IDisconListService
    {
        List<DisconListModel> GetSubForDisconList(int nummonths, string status, string route);
    }
}
