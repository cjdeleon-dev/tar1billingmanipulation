using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAR1ORDATA.DataService.DBService
{
    public interface IDBService
    {
        bool IsValidConnectionString(string connectionstring);
    }
}
