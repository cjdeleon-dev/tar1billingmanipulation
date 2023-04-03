using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ConsumerZeroKWHModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string MeterNo { get; set; }
        public string PoleId { get; set; }
        public string Type { get; set; }
        public int ReadKWH { get; set; }
    }
}