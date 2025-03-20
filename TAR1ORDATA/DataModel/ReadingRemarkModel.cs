using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ReadingRemarkModel
    {
        public string ConsumerId { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public string MeterSerialNo { get; set; }
        public string ErrText { get; set; }
        public string Remark { get; set; }
    }
}