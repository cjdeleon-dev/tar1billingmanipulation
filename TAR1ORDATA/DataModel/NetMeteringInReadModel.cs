using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class NetMeteringInReadModel
    {
        public string ConsumerId { get; set; }
        public string BillPeriod { get; set; }
        public int NetImport { get; set; }
        public int NetExport { get; set; }
        public decimal ActualDemand { get; set; }
    }
}