using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.TopActiveDisconService
{
    public interface ITopActiveDisconService
    {
        List<ConsumerStatusModel> GetAllStatus();
        List<ConsumerTownModel> GetAllTown();
        List<HighArrearsConsumerModel> GetTopHundred(string stat, string town, string top);
    }
}
