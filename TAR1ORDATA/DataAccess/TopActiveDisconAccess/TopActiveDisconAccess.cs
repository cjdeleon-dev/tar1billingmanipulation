using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.TopActiveDisconAccess
{
    public class TopActiveDisconAccess : ConnectionAccess, ITopActiveDisconAccess
    {
        public List<ConsumerStatusModel> GetAllStatus()
        {
            List<ConsumerStatusModel> lststatm = new List<ConsumerStatusModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select * from (VALUES(1, 'Active'),(2, 'Disconnected')) q(id,cstatus);";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lststatm.Add(new ConsumerStatusModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                CStatus = dr["cstatus"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lststatm = null;
                    }
                }
                catch (Exception ex)
                {
                    lststatm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lststatm;
        }

        public List<ConsumerTownModel> GetAllTown()
        {
            List<ConsumerTownModel> lsttownm = new List<ConsumerTownModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select * " +
                                               "from(VALUES(1, 'San Manuel'), (2, 'Moncada'), (3, 'Paniqui'), " +
                                               "(4, 'Ramos'), (5, 'San Clemente'), (6, 'Camiling'), " +
                                               "(7, 'Mayantoc'), (8, 'Sta. Ignacia'), (9, 'Gerona'), " +
                                               "(10, 'Victoria'), (11, 'Pura'), (12, 'Anao, Tarlac'), " +
                                               "(13, 'Nampicuan, Nueva Ecija'), (14, 'Cuyapo, Nueva Ecija'), " +
                                               "(15, 'San Jose, Tarlac') " +
                                               ") q(id, ctown); ";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsttownm.Add(new ConsumerTownModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                CTown = dr["ctown"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lsttownm = null;
                    }
                }
                catch (Exception ex)
                {
                    lsttownm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lsttownm;
        }

        public List<HighArrearsConsumerModel> GetTopHundred(string stat, string town, string top)
        {
            List<HighArrearsConsumerModel> lsttophundred = new List<HighArrearsConsumerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_Top300HighArrears";

                da.SelectCommand.Parameters.AddWithValue("@status", stat.ToUpper().Substring(0,1));
                da.SelectCommand.Parameters.AddWithValue("@town", town);
                da.SelectCommand.Parameters.AddWithValue("@top", Convert.ToInt32(top));

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsttophundred.Add(new HighArrearsConsumerModel
                            {
                               AccountNumber = dr["consumerid"].ToString(),
                               Amount = Convert.ToDouble(dr["Amount"]),
                               VAT = Convert.ToDouble(dr["VAT"]),
                               NumOfMonths = Convert.ToInt32(dr["NumberOfMonths"]),
                               ConsumerType = dr["type"].ToString(),
                               AccountName = dr["name"].ToString(),
                               Address = dr["address"].ToString(),
                               Town = dr["town"].ToString(),
                               MeterNumber = dr["mtrnumber"].ToString(),
                               PoleNumber = dr["poleno"].ToString()
                            });
                        }
                    }
                    else 
                    {
                        lsttophundred = null;
                    }
                }
                catch (Exception ex)
                {
                    lsttophundred = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lsttophundred;
            
        }
    }
}