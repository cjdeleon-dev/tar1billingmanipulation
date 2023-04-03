using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ConsumerStatusLogModel
    {
        public string AccountNo { get; set; }
        public string DateEntry { get; set; }
        public string StatusFrom { get; set; }
        public string ChangeStatusFrom { get; set; }
        public string StatusTo { get; set; }
        public string ChangeStatusTo { get; set; }
        public string Reason { get; set; }
        public string EntryUser { get; set; }
        public string ActionDate { get; set; }
        public string DTDRead { get; set; }
    }
}