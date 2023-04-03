using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemConsMeterSpecModel
    {
        public string AccountNo { get; set; }
        public string MeterSerialNo { get; set; }
        public string MeterSealNo { get; set; }
        public string MeterSideSealNo { get; set; }
        public string MeterType { get; set; }
        public string MeterBrand { get; set; }
        public string DateInstalled { get; set; }
        public string PrevMeterSerialNo { get; set; }
        public int MeterAmp { get; set; }
        public int MeterDial { get; set; }
        //Flags
        public string SCFlag { get; set; }
        public string PKFlag { get; set; }
        public string WOFlag { get; set; }
        public string PNFlag { get; set; }
        public string CLRSR { get; set; }
    }
}