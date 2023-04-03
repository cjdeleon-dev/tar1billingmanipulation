using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class DTDWithCurrBillQueries
    {
        public static readonly string getAllDTDWithCurrBills = "declare @billperiod as varchar(6); " +
                                                               "select @billperiod = arsbillperiod.billperiod from arsbillperiod where seq = 0; " +
                                                               "select cons.consumerid, cons.name, cons.address, cons.mtrserialno, ISNULL(cons.poleid,'') poleid, " +
                                                               "rtrim(typ.description) [type], trxamount [amount] " +
                                                               "from arsconsumer cons inner join arstrxhdr trx " +
                                                               "on cons.consumerid=trx.consumerid " +
                                                               "inner join arstype typ on cons.consumertypeid=typ.consumertypeid " +
                                                               "where statusid='D' and billperiod = @billperiod and trx.trxid='EB' and trx.trxamount>0;";
    }
}