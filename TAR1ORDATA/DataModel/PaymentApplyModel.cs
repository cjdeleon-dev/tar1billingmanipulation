using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PaymentApplyModel
    {
        public string BillingDate { get; set; }
        public string Remarks { get; set; }
        public string DueDate { get; set; }
        public string Amount { get; set; }
        public string VAT { get; set; }
        public string Surcharge { get; set; }
        public string Total { get; set; }
        public string SCDisc { get; set; }
    }
}