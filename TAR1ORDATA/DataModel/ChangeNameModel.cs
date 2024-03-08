using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ChangeNameModel
    {
        public int Id { get; set; }
        public string ApplicationDate { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string MemberId { get; set; }
        public string MemberDate { get; set; }
        public string SequenceNo { get; set; }
        public bool IsDied { get; set; }
        public string NewName { get; set; }
        public string NewMemberId { get; set; }
        public string NewMemberDate { get; set; }
        public string Birthday { get; set; }
        public string ContactNo { get; set; }
        public string Relationship { get; set; }
        public string Reason { get; set; }
        //public bool ForWithdrawOld { get; set; }
        //public bool ForWithdrawNew { get; set; }
        //public bool ForRetention { get; set; }
        public int ChangeNameTypeId { get; set; }
        public string ChangeNameDesc { get; set; }
        //public string Remarks { get; set; }
        public bool IsRemDeathCert { get; set; }
        public bool IsRemAuthLetter { get; set; }
        public bool IsRemDeedOfSale { get; set; }
        public bool IsRemLetterOfReq { get; set; }
        public bool IsRemOther { get; set; }
        public string RemOtherText { get; set; }
        public string RptRemark { get; set; }
        public string MadeById { get; set; }
    }
}