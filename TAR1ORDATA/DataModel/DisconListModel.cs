using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class DisconListModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string MeterNo { get; set; }
        public string FirstBill { get; set; }
        public string LastBill { get; set; }
        public int NoOfMonths { get; set; }
        public double Due { get; set; }
        public string Remark { get; set; }
    }
}