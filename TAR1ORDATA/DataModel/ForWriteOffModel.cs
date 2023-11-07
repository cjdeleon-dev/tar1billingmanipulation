using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ForWriteOffModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string StatusId { get; set; }
        public string TypeId { get; set; }
        public double TrxBalance { get; set; }
        public double VATBalance { get; set; }
        public double TotalBalance { get; set; }
    }
}