using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace TAR1ORDATA.DataAccess.AuditTrailAccess
{
    public class AuditTrailAccess : ConnectionAccess, IAuditTrailAccess
    {
        public bool LogtoAuditTrail(AuditTrailModel atm)
        {
            bool result = false;

            SqlConnection conn = new SqlConnection(this.ConnectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into tblAuditTrail(processtypeid,tableaffected,processmade,madeby,madedatetime) " +
                                  "values(@processtypeid,@tblaffected,@processmade,@madeby,getdate());";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@processtypeid", atm.ProcessTypeId);
                cmd.Parameters.AddWithValue("@tblaffected", atm.TableAffected);
                cmd.Parameters.AddWithValue("@processmade", atm.ProcessMade);
                cmd.Parameters.AddWithValue("@madeby", atm.MadeById);

                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

            return result;
        }
    }
}