using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class NetMeteringPrevReadModel
    {
        public string BillPeriod { get; set; }
        public int PrevImp { get; set; }
        public int PrevExp { get; set; }
        public int PrevRec { get; set; }
    }
}