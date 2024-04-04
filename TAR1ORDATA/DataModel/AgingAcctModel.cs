using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class AgingAcctModel
    {
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string ConsumerType { get; set; }
        public double Days30 { get; set; }
        public double Days60 { get; set; }
        public double Days90 { get; set; }
        public double Days180 { get; set; }
        public double Days240 { get; set; }
        public double Days360{ get; set; }
        public double AboveDays365 { get; set; } 
    }
}