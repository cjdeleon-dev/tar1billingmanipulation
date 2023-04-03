using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.ChangeStatusAccess
{
    public class ChangeStatusAccess : ConnectionAccess, IChangeStatusAccess
    {
        public List<ConsumerModel> GetAllActiveConsumers()
        {
            List<ConsumerModel> lstcm = new List<ConsumerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_GetAllActiveConsumers";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstcm.Add(new ConsumerModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                AccountName = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                MeterNo = dr["mtrserialno"].ToString(),
                                PoleId = dr["poleid"].ToString(),
                                Status = dr["status"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstcm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcm;
        }

        public List<ConsumerModel> GetAllDisconConsumers()
        {
            List<ConsumerModel> lstcm = new List<ConsumerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_GetAllDisconConsumers";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstcm.Add(new ConsumerModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                AccountName = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                MeterNo = dr["mtrserialno"].ToString(),
                                PoleId = dr["poleid"].ToString(),
                                Status = dr["status"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstcm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcm;
        }

        public List<StatusModel> GetAllStatus(string exceptStatus)
        {
            List<StatusModel> lsstat = new List<StatusModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select statusid [StatusId], description [Description] from arsstatus where statusid <> @statusid";

                da.SelectCommand.Parameters.AddWithValue("@statusid", exceptStatus);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsstat.Add(new StatusModel
                            {
                                StatusId = dr["StatusId"].ToString(),
                                Description = dr["Description"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lsstat = null;
                    }
                }
                catch (Exception ex)
                {
                    lsstat = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lsstat;
        }

        public List<ConsumerStatusLogModel> GetStatusLogByConsumerid(string consumerid)
        {
            List<ConsumerStatusLogModel> lstcslm = new List<ConsumerStatusLogModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select consumerid,rtrim(c.description)[changefrom],rtrim(d.description)[changeto],rtrim(reason)[reason]," +
                                               "rtrim(b.initials)[user],trxdate[entrydate],actdate[actiondate],rtrim(dtdread)[dtdread] " +
                                               "from arsstatuslog a inner join secuser b on a.userid = b.userid " +
                                               "inner join arsstatus c on a.oldstatusid = c.statusid " +
                                               "inner join arsstatus d on a.newstatusid = d.statusid " +
                                               "where consumerid = @consumerid order by trxdate desc";

                da.SelectCommand.Parameters.AddWithValue("@consumerid", consumerid);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstcslm.Add(new ConsumerStatusLogModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                ChangeStatusFrom = dr["changefrom"].ToString(),
                                ChangeStatusTo = dr["changeto"].ToString(),
                                Reason = dr["reason"].ToString(),
                                EntryUser = dr["user"].ToString(),
                                DateEntry = dr["entrydate"].ToString(),
                                ActionDate = dr["actiondate"].ToString().Trim()!=""?Convert.ToDateTime(dr["actiondate"].ToString()).ToShortDateString():"",
                                DTDRead = dr["dtdread"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstcslm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcslm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcslm;
        }

        public bool SetStatusOfConsumerId(ConsumerStatusLogModel cslm)
        {
            bool result = false;

            SqlConnection conn = new SqlConnection(this.ConnectionString);
            SqlCommand cmd = new SqlCommand();

            conn.Open();

            SqlTransaction trans = conn.BeginTransaction();
            
            try
            {
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_SetStatusOfConsumerById";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@accountno", cslm.AccountNo);
                cmd.Parameters.AddWithValue("@changestatusfr", cslm.StatusFrom);
                cmd.Parameters.AddWithValue("@changestatusto", cslm.StatusTo);
                cmd.Parameters.AddWithValue("@reason", cslm.Reason);
                cmd.Parameters.AddWithValue("@entryuser", cslm.EntryUser);
                cmd.Parameters.AddWithValue("@actiondate", cslm.ActionDate);
                if(cslm.DTDRead!=null)
                    cmd.Parameters.AddWithValue("@dtdread", cslm.DTDRead);
                else
                    cmd.Parameters.AddWithValue("@dtdread", DBNull.Value);

                cmd.ExecuteNonQuery();
                result = true;
                trans.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                trans.Rollback();
            }
            finally
            {
                conn.Close();
                trans.Dispose();
                cmd.Dispose();
            }

            return result;
        }
    }
}