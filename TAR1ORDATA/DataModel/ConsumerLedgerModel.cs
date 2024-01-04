using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ConsumerLedgerModel
    {
        public int TrxSeqId { get; set; }
        public string TrxDate { get; set; }
        public string Trx { get; set; }
        public string Period { get; set; }
        public int Prev { get; set; }
        public int Curr { get; set; }
        public int KWh { get; set; }
        public double DMU { get; set; }
        public double TrxAmount { get; set; }
        public double TrxBalance { get; set; }
        public double VAT { get; set; }
        public double VATBalance { get; set; }
        public double TotalTrxBalance { get; set; }
        public double TotalVatBalance { get; set; }
        public int Months { get; set; }
        public bool isBalance { get; set; }
    }
}