using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.ChangeStatusAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.ChangeStatusService
{
    public class ChangeStatusService: IChangeStatusService
    {
        IChangeStatusAccess icsa;
        public ChangeStatusService()
        {
            icsa = new ChangeStatusAccess();
        }

        public List<ConsumerModel> GetAllActiveConsumers()
        {
            return icsa.GetAllActiveConsumers();
        }

        public List<ConsumerModel> GetAllDisconConsumers()
        {
            return icsa.GetAllDisconConsumers();
        }

        public List<StatusModel> GetAllStatus(string exceptStatus)
        {
            return icsa.GetAllStatus(exceptStatus);
        }

        public List<ConsumerStatusLogModel> GetStatusLogByConsumerid(string consumerid)
        {
            return icsa.GetStatusLogByConsumerid(consumerid);
        }

        public bool SetStatusOfConsumerId(ConsumerStatusLogModel cslm)
        {
            return icsa.SetStatusOfConsumerId(cslm);
        }
    }
}