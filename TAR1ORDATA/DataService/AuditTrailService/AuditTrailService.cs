using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.AuditTrailAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.AuditTrailService
{
    public class AuditTrailService : IAuditTrailService
    {
        IAuditTrailAccess iata;

        public AuditTrailService()
        {
            this.iata = new AuditTrailAccess();
        }

        public bool LogtoAuditTrail(AuditTrailModel atm)
        {
            return iata.LogtoAuditTrail(atm);
        }
    }
}