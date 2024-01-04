using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class AppCNMemModel
    {
        public int RefId { get; set; }
        public string NewName { get; set; }
        public string OldMemberId { get; set; }
        public string OldMemDate { get; set; }
        public string Remark { get; set; }
        public string UpdatedBy { get; set; }
    }
}