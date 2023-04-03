using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class ClaimedBurialQueries
    {
        public static readonly string sqlGetAllClaimedBurialConsumers = "select consumerid [accountno],name [name],address [address],dateclaimburial [claimeddate] " +
                                                                        "from arsconsumer where dateclaimburial is not null " +
                                                                        "order by consumerid;";
    }
}