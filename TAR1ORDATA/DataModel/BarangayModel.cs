using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class BarangayModel
    {
        public int Id { get; set; }
        public string Barangay { get; set; }
        public int MunicipalityId { get; set; }
        public string Municipality { get; set; }
        public string Province { get; set; }
    }
}