using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ChangeNameAppliedModel
    {
        public int Id { get; set; }
        public string AppDate { get; set; }
        public string AccountNo { get; set; }
        public string OldName { get; set; }
        public string Address { get; set; }
        public string OldMemberId { get; set; }
        public string OldMemberDate { get; set; }
        public string NewName { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
    }
}