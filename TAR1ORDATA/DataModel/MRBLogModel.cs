using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MRBLogModel
    {
        public string Name { get; set; }
        public string RouteId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public double Total { get; set; }
    }
}