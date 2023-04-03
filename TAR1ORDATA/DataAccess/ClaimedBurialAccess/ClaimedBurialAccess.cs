using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Queries;

namespace TAR1ORDATA.DataAccess.ClaimedBurialAccess
{
    public class ClaimedBurialAccess : ConnectionAccess, IClaimedBurialAccess
    {
        public List<ClaimedBurialModel> GetAllClaimedBurialConsumers()
        {
            List<ClaimedBurialModel> lstcbm = new List<ClaimedBurialModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = ClaimedBurialQueries.sqlGetAllClaimedBurialConsumers;

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstcbm.Add(new ClaimedBurialModel
                            {
                                AccountNo = dr["accountno"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                ClaimedDate = Convert.ToDateTime(dr["claimeddate"]).ToShortDateString()
                            });
                        }
                    }
                    else
                    {
                        lstcbm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcbm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcbm;
        }
    }
}