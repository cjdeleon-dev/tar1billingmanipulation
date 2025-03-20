using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PoleRentalPaymentModel
    {
        public string ORNumber { get; set; }
        public string Payee { get; set; }
        public string Address { get; set; }
        public string TrxDate { get; set; }
        public double Amount { get; set; }
        public double VAT { get; set; }
        public double TotalAmount { get; set; }
    }
}