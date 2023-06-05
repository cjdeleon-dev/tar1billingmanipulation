using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.ChangeStatusService
{
    public interface IChangeStatusService
    {
        List<ConsumerModel> GetAllActiveConsumers();
        List<ConsumerModel> GetAllDisconConsumers();

        ProcessResultModel SetStatusOfConsumerId(ConsumerStatusLogModel cslm);

        List<ConsumerStatusLogModel> GetStatusLogByConsumerid(string consumerid);
        List<StatusModel> GetAllStatus(string exceptStatus);
    }
}
