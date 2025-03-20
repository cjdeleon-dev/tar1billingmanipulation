using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class CompareMasterPaymentModel
    {
        public string RealConsumerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FakeConsumerId { get; set; }
    }
}