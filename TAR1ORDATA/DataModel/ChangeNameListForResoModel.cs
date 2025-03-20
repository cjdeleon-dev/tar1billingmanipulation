using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class ChangeNameListForResoModel
    {
        public int Id { get; set; }
        public string ApplicationDate { get; set; }
        public string NameOld { get; set; }
        public string Address { get; set; }
        public string ORNoOld { get; set; }
        public string ORDateOld { get; set; }
        public string NameNew { get; set; }
        public string ORNoNew { get; set; }
        public string ORDateNew { get; set; }
        public string AccountNo { get; set; }
        public string Remarks { get; set; }
        public string RptRemark { get; set; }
        public string DateApproved { get; set; }
    }
}