using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace TAR1ORDATA.DataModel
{
    public class BurialAppValModel
    {
        public int Id { get; set; }
        public string ConsumerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AppValDate { get; set; }
    }
}