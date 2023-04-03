using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemConsLoadInfoModel
    {
        public string AccountNo { get; set; }
        public double FlatRate { get; set; }
        public double FlatDemand { get; set; }
        public double Coreloss { get; set; }
        public double KVALoad { get; set; }
        public double TSFRental { get; set; }
        public double Multiplier { get; set; }
        public int TSFCount { get; set; }
    }
}