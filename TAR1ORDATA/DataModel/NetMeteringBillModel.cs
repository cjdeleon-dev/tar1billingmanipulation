using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class NetMeteringBillModel
    {
        public decimal EnergyAmount { get; set; }
        public decimal DemandAmount { get; set; }
        public decimal BillAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetBillAmount { get; set; }
        public decimal TotalCurrentBill { get; set; }
    }
}