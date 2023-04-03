using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.Enums;

namespace TAR1ORDATA.DataModel
{
    public class TransactionSearchModel
    {
        public string NewPrefix { get; set; }
        public string ORNumberFrom { get; set; }
        public string ORNumberTo { get; set; }
        public Int32 Addend { get; set; }
        public Int32 Subtrahend { get; set; }

        public ORProcessOperators UsedOperator { get; set; }
    }
}