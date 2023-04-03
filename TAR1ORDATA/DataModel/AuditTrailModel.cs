using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class AuditTrailModel
    {
        public int Id { get; set; }
        public int ProcessTypeId { get; set; } //1-selection, 2-insertion, 3-modification, 4-deletion
        public string TableAffected { get; set; }
        public string ProcessMade { get; set; }
        public string MadeById { get; set; }
        public string MadeDateTime { get; set; }
    }
}