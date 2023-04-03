using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TAR1ORDATA.Queries;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.UserLoginAccess
{
    public class UserLoginAccess : ConnectionAccess, IUserLoginAccess
    {
        public List<RoleModel> GetRolesByUserId(string userid)
        {
            List<RoleModel> lstRoles = new List<RoleModel>();

            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
            da.SelectCommand.Connection.Open();

            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.CommandText = AuthenticationQueries.sqlGetRolesByUserId;

            da.SelectCommand.Parameters.AddWithValue("@userid", userid);

            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstRoles.Add(new RoleModel
                        {
                            RoleCode = dr["rolecode"].ToString(),
                            Description = dr["roledescription"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                lstRoles = null;
            }
            finally
            {
                da.SelectCommand.Connection.Close();
                da.Dispose();
            }

            return lstRoles;
        }

        public bool GetUserByIdAndPass(string userid, string pass)
        {
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = AuthenticationQueries.sqlGetUserByIdAndPass;

                da.SelectCommand.Parameters.AddWithValue("@userid", userid);
                da.SelectCommand.Parameters.AddWithValue("@pass", pass);

                DataTable dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        Globals.GlobalVariables.glUserId = dt.Rows[0]["userid"].ToString();
                        Globals.GlobalVariables.glName = dt.Rows[0]["name"].ToString();
                        Globals.GlobalVariables.glWorkgroupId = dt.Rows[0]["workgroupid"].ToString();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }catch(Exception ex)
                {
                    return false;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }
        }
    }
}