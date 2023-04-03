using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ConsumerModel
    {
        public string Id { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string MeterNo { get; set; }
        public string SeqNo { get; set; }
        public string PoleId { get; set; }
        public string Status { get; set; }
        public string MemberId { get; set; }
        public string ORDate { get; set; }
    }
}