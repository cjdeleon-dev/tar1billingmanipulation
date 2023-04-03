using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataAccess.DBAccess
{
    public class DBAccess : IDBAccess
    {
        public bool IsValidConnectionString(string connectionstring)
        {
            bool result = false;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(connectionstring);
                try
                {
                    cmd.Connection.Open();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                
            }
            return result;
        }
    }
}