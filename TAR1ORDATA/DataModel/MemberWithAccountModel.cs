using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TAR1ORDATA.DataModel
{
    public class MemberWithAccountModel
    {
        //     @isbusiness bit,
        //   @lname varchar(255),
        //   @fname varchar(255),
        //   @mname varchar(255),
        //   @suffix varchar(255),
        //   @businessname varchar(255),
        //   @membertypeid int,
        //   @barangay varchar(255),
        //   @municipality varchar(255),
        //   @officeid int,
        //   @memberid varchar(10),
        //   @memberdate date,
        //   @madeby varchar(5),
        //@accountno varchar(10),
        //@consumertype varchar(1),
        //@isexistacct bit

        public bool IsBusiness { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string Suffix { get; set; }
        public string BusinessName { get; set; }
        public int MemberTypeId{ get; set; }
        public string Barangay { get; set; }
        public string Municipality { get; set; }
        public int OfficeId { get; set; }
        public string MemberId { get; set; }
        public string MemberDate { get; set; }
        public string MadeBy { get; set; }
        public string AccountNo { get; set; }
        public string ConsumerType { get; set; }
        public bool IsExistAccount { get; set; }
    }
}