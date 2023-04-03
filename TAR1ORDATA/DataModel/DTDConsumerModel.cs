using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class DTDConsumerModel
    {
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Reason { get; set; }
        public string LastReading { get; set; }
        public string EncoderId { get; set; }
        public string Encoder { get; set; }
        public string ActDate { get; set; }
    }
}