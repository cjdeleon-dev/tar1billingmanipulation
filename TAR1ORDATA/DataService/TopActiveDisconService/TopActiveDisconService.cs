using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.TopActiveDisconAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.TopActiveDisconService
{
    public class TopActiveDisconService : ITopActiveDisconService
    {
        ITopActiveDisconAccess itada;

        public TopActiveDisconService()
        {
            this.itada = new TopActiveDisconAccess();
        }

        public List<ConsumerStatusModel> GetAllStatus()
        {
            return itada.GetAllStatus();
        }

        public List<ConsumerTownModel> GetAllTown()
        {
            return itada.GetAllTown();
        }

        public List<HighArrearsConsumerModel> GetTopHundred(string stat, string town, string top)
        {
            return itada.GetTopHundred(stat, town, top);
        }
    }
}