using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.TopActiveDisconAccess
{
    public interface ITopActiveDisconAccess
    {
        List<ConsumerStatusModel> GetAllStatus();
        List<ConsumerTownModel> GetAllTown();
        List<HighArrearsConsumerModel> GetTopHundred(string stat, string town, string top);
    }
}
