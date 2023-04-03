﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.ForWriteOffAccess
{
    public interface IForWriteOffAccess
    {
        List<ForWriteOffModel> GetForWriteOffList(int yr,string area);
        List<YearOfModel> GetAllYearOf();
    }
}
