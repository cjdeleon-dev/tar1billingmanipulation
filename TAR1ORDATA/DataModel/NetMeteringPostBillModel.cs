using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class NetMeteringPostBillModel
    {
        public string ConsumerId { get; set; }
        public int PrevImp { get; set; }
        public int CurrImp { get; set; }
        public int NetImp { get; set; }
        public int PrevExp { get; set; }
        public int CurrExp { get; set; }
        public int NetExp { get; set; }
        public int PrevRec { get; set; }
        public int CurrRec { get; set; }
        public double Demand { get; set; }
        public string TrxDate { get; set; }
        public string BillPeriod { get; set; }
        public string EntryUser { get; set; }
    }
}