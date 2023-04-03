using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class SumDTDRptHeaderModel
    {
        public int Id { get; set; }
        public string DateTimeGenerate { get; set; }
        public string GenerateUserId { get; set; }
        public string CheckByUserId { get; set; }
        public string CheckBy { get; set; }
        public string NotedByUserId { get; set; }
        public string NotedBy { get; set; }
        public string RouteId { get; set; }
        public string OfficeId { get; set; }
        public string ActionDate { get; set; }
    }
}