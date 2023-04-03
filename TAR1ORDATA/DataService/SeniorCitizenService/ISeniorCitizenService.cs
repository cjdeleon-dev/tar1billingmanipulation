using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.SeniorCitizenService
{
    public interface ISeniorCitizenService
    {
        List<SeniorCitizenModel> GetAllSeniorCitizens();
    }
}
