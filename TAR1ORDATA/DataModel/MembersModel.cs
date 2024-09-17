using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MembersModel
    {
        public int Id { get; set; }
        public bool IsBusiness  { get; set; }
        public string BusinessName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public int MemberTypeId { get; set; }
        public string MemberType { get; set; }
        public string MemberId { get; set; }
        public string MemberDate { get; set; }
        public string Barangay { get; set; }
        public string Municipality { get; set; }
    }
}