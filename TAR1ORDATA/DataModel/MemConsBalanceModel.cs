using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemConsBalanceModel
    {
        public string AccountNumber { get; set; }
        public string BillPeriod { get; set; }
        public double TrxBalance { get; set; }
        public double VATBalance { get; set; }
        public double Surcharge { get; set; }
        public double TotalAmount { get; set; }
        public int Months { get; set; }
        public double PayAmount { get; set; }
    }
}