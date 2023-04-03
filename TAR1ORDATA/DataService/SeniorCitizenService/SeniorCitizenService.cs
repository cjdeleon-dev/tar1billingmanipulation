using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.SeniorCitizenAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.SeniorCitizenService
{
    public class SeniorCitizenService: ISeniorCitizenService
    {
        ISeniorCitizenAccess isca;

        public SeniorCitizenService()
        {
            this.isca = new SeniorCitizenAccess();
        }

        public List<SeniorCitizenModel> GetAllSeniorCitizens()
        {
            return isca.GetAllSeniorCitizens();
        }
    }
}