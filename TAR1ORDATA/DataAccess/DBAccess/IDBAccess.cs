using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR1ORDATA.DataAccess.DBAccess
{
    public interface IDBAccess
    {
        bool IsValidConnectionString(string connectionstring);
    }
}
