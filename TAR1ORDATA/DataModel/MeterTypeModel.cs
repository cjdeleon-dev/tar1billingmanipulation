using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MeterTypeModel
    {
        public int Id { get; set; }
        public string MeterType { get; set; }
        public int MeterBrandId { get; set; }
        public string MeterBrand { get; set; }
        public bool IsActive { get; set; }
    }
}