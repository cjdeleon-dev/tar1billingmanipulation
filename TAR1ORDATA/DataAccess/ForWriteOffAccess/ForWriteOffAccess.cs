using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Queries;

namespace TAR1ORDATA.DataAccess.ForWriteOffAccess
{
    public class ForWriteOffAccess : ConnectionAccess, IForWriteOffAccess
    {
        public List<YearOfModel> GetAllYearOf()
        {
            List<YearOfModel> lstyom = new List<YearOfModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = ForWriteOffQueries.sqlGetYearOf;

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstyom.Add(new YearOfModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                YearOf = Convert.ToInt32(dr["yearof"])
                            });
                        }
                    }
                    else
                    {
                        lstyom = null;
                    }
                }
                catch (Exception ex)
                {
                    lstyom = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstyom;
        }

        public List<ForWriteOffModel> GetForWriteOffList(int yr, string area)
        {
            List<ForWriteOffModel> lstfwom = new List<ForWriteOffModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = ForWriteOffQueries.sqlGetAllForWriteOff;

                da.SelectCommand.Parameters.AddWithValue("@area", area);
                da.SelectCommand.Parameters.AddWithValue("@lastbillperiod", yr);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstfwom.Add(new ForWriteOffModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                AccountName = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                StatusId = dr["statusid"].ToString(),
                                TypeId = dr["consumertypeid"].ToString(),
                                TrxBalance = Convert.ToDouble(dr["trxbalance"]),
                                VATBalance = Convert.ToDouble(dr["vatbalance"]),
                                TotalBalance = Convert.ToDouble(dr["totalbalance"])
                            });
                        }
                    }
                    else
                    {
                        lstfwom = null;
                    }
                }
                catch (Exception ex)
                {
                    lstfwom = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstfwom;
        }
    }
}