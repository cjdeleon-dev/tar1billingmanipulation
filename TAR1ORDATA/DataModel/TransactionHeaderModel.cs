using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class TransactionHeaderModel
    {
        public string ORNumber { get; set; }
        public string Payee { get; set; }
        public string AccountNumber { get; set; }
        public string NewORNumber { get; set; }
        public string TransactionDate { get; set; }
        public string EntryDate { get; set; }
    }
}