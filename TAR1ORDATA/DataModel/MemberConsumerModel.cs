using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class MemberConsumerModel
    {
        //Profile Information
        public string ConsumerId { get; set; }
        public string ConsumerTypeId { get; set; }
        public string ConsumerType { get; set; }
        public string ConsumerClass { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PoleId { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string MemberOR { get; set; }
        public string MemberDate { get; set; }
        public string BookNo { get; set; }
        public string SequenceNo { get; set; }
        public string AreaId { get; set; }
        public string Area { get; set; }
        public string OfficeId { get; set; }
        public string Office { get; set; }
        public bool IsClaimedBurial { get; set; }
        public string ClaimedBurialDate { get; set; }
        //Load Information
        public double FlatRate { get; set; }
        public double FlatDemand { get; set; }
        public double Coreloss { get; set; }
        public double KVALoad { get; set; }
        public double TSFRental { get; set; }
        public double Multiplier { get; set; }
        public int TSFCount { get; set; }
        //Meter Specification
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