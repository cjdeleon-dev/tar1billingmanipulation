using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class TempConsumerModel
    {
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MeterNo { get; set; }
        public string PoleNo { get; set; }
        public double TrxBalance { get; set; }
        public double VATBalance { get; set; }
        public string Status { get; set; }
        public string DateInstalled { get; set; }
    }
}