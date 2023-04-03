using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ChangedStatusModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string DateChange { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Reason { get; set; }
    }
}