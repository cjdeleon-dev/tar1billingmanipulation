using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PaymentHeaderModel
    {
        public string ORNumber { get; set; }
        public string ConsumerId { get; set; }
        public string Payee { get; set; }
        public string Address { get; set; }
        public string ConsumerType { get; set; }
        public string ConsumerStatus { get; set; }
        public string ModeOfPayment { get; set; }
        public string Office { get; set; }
        public string TIN { get; set; }
        public string TrxDate { get; set; }
        public string CheckNumber { get; set; }
        public string CheckName { get; set; }
        public string Bank { get; set; }
        public double TotalAmount { get; set; }
        public double Tendered { get; set; }
        public double Change { get; set; }
        public string TrxStatus { get; set; }
        public string EntryUser { get; set; }
        public string EntryDate { get; set; }
    }
}