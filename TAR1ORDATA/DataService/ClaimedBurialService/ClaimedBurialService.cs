using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.ClaimedBurialAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.ClaimedBurialService
{
    public class ClaimedBurialService : IClaimedBurialService
    {
        IClaimedBurialAccess icba;

        public ClaimedBurialService()
        {
            this.icba = new ClaimedBurialAccess();
        }

        public List<ClaimedBurialModel> GetAllClaimedBurialConsumers()
        {
            return icba.GetAllClaimedBurialConsumers();
        }
    }
}