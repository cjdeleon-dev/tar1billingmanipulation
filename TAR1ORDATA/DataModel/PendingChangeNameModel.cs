using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PendingChangeNameModel
    {
        public int RefID { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string AppDate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
    }
}