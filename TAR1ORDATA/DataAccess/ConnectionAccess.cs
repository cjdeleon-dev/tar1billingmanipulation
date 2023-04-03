using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataAccess
{
    public class ConnectionAccess
    {
        protected string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["getconnstr"].ToString();
            }
        }
    }
}