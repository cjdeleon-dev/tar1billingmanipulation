using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PaymentDetailModel
    {
        public string TrxId { get; set; }
        public string TrxDesc { get; set; }
        public double TrxAmount { get; set; }
        public double VAT { get; set; }
        public double Amount { get; set; }
    }
}