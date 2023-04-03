using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ConsumerHighKwhModel
    {
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PoleId { get; set; }
        public string MeterNo { get; set; }
        public int KwH { get; set; }
        public double Amount { get; set; }
    }
}