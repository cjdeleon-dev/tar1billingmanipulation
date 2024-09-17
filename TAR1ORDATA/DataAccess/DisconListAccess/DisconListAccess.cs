using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.DisconListAccess
{
    public class DisconListAccess : ConnectionAccess, IDisconListAccess
    {
        public List<DisconListModel> GetSubForDisconList(int nummonths, string status, string route)
        {
            List<DisconListModel> lstdlm = new List<DisconListModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();
                da.SelectCommand.CommandTimeout = 100000000;

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_GenerateSubjForDisconV2";

                da.SelectCommand.Parameters.AddWithValue("@filternum", nummonths);
                da.SelectCommand.Parameters.AddWithValue("@status", Convert.ToInt32(status));
                da.SelectCommand.Parameters.AddWithValue("@route", route);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstdlm.Add(new DisconListModel
                            {
                                AccountNo = dr["AccountNo"].ToString(),
                                AccountName = dr["Name"].ToString(),
                                Address = dr["Address"].ToString(),
                                MeterNo = dr["MeterNo"].ToString(),
                                FirstBill = dr["FromBP"].ToString(),
                                LastBill = dr["ToBP"].ToString(),
                                NoOfMonths = Convert.ToInt32(dr["NumberOfMonths"]),
                                Due = Convert.ToDouble(dr["TotalAmountDue"]),
                                Remark = dr["Remarks"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstdlm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstdlm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstdlm;
        }
    }
}