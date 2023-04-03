using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class DTDWithCurrentBillModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string MeterNumber { get; set; }
        public string PoleId { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public double VAT { get; set; }
        public int NumOfMonths { get; set; }
    }
}