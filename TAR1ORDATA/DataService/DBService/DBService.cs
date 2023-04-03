using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.DBAccess;

namespace TAR1ORDATA.DataService.DBService
{
    public class DBService : IDBService
    {
        IDBAccess idbaccess;
        public DBService()
        {
            this.idbaccess = new DBAccess();
        }
        public bool IsValidConnectionString(string connectionstring)
        {
            return idbaccess.IsValidConnectionString(connectionstring);
        }
    }
}