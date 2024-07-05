using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemberAccountModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string AccountNo { get; set; }
        public string Address { get; set; }
        public bool IsPrimary { get; set; }
    }
}