using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.ForWriteOffAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.ForWriteOffService
{
    public class ForWriteOffService : IForWriteOffService
    {
        IForWriteOffAccess ifwoa;

        public ForWriteOffService()
        {
            this.ifwoa = new ForWriteOffAccess();
        }

        public List<YearOfModel> GetAllYearOf()
        {
            return ifwoa.GetAllYearOf();
        }

        public List<ForWriteOffModel> GetForWriteOffList(int yr, string area)
        {
            return ifwoa.GetForWriteOffList(yr,area);
        }
    }
}