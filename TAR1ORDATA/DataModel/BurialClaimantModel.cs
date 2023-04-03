using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class BurialClaimantModel
    {
        public int Id { get; set; }
        public int BurialHeaderId { get; set; }
        public string MCDateOfDeath { get; set; }
        public string MCCauseOfDeath { get; set; }
        public string ClaimantName { get; set; }
        public string ClaimantAddress { get; set; }
        public string Relationship { get; set; }
        public string ContactNum { get; set; }
        public string ScreenedBy { get; set; }
        public string ScreenedByPos { get; set; }
        public string RecommendedBy { get; set; }
        public string RecommendedByPos { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedByPos { get; set; }
    }
}
