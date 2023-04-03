using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class SumDTDRptDetailModel
    {
        public int Id { get; set; }
        public int SumDTDRptHeaderId { get; set; }
        public string ConsumerId { get; set; }
        public string Reason { get; set; }
        public string LastReading { get; set; }
        public string EncodeUserId { get; set; }
    }
}