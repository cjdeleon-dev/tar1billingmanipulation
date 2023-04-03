using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemConsProfileModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountStat { get; set; }
        public string AccountAdd { get; set; }
        public string AccountPoleId { get; set; }
        public string AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public string MemberOR { get; set; }
        public string MemberORDate { get; set; }
        public string BookNo { get; set; }
        public string SeqNo { get; set; }
        public string AreaId { get; set; } //Billing Group
        public string Area { get; set; }
        public string OfficeId { get; set; } //Collecting Area
        public string Office { get; set; }
        public bool IsClaimedBurial { get; set; }
        public string ClaimedBurialDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}