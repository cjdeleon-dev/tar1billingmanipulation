using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TAR1ORDATA.Queries;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Enums;

namespace TAR1ORDATA.DataAccess.HomeAccess
{
    public class HomeAccess : ConnectionAccess, IHomeAccess
    {
        public bool isNewORVacant(string ORFrom, string ORTo, int AddDiffValue, bool isAddition)
        {
            bool result = true;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(this.ConnectionString);
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                
                int valtoadddiff = AddDiffValue;
                int lengthor = ORFrom.Length;
                string prefix = ORFrom.Substring(1, 1);
                string frOR = string.Empty;
                string toOR = string.Empty;

                if (isAddition)
                {
                    frOR = prefix + (Convert.ToInt32(ORFrom.Substring(2)) + valtoadddiff).ToString();
                    toOR = prefix + (Convert.ToInt32(ORTo.Substring(2)) + valtoadddiff).ToString();
                }
                else
                {
                    frOR = prefix + (Convert.ToInt32(ORFrom.Substring(2)) - valtoadddiff).ToString();
                    toOR = prefix + (Convert.ToInt32(ORTo.Substring(2)) - valtoadddiff).ToString();
                }

                cmd.CommandText = "select top 1 ornumber from arspaytrxhdr where ornumber between '" 
                                + frOR + "' and '" + toOR + "' and LEN(RTRIM(ornumber))=" + lengthor + ";";

                try
                {
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                            result = false; //kapag merong existing or
                        else
                            result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        public List<TransactionHeaderModel> ListNewPrefixOfORRange(TransactionSearchModel tsm)
        {
            List<TransactionHeaderModel> lstthm = new List<TransactionHeaderModel>();
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.CommandType = CommandType.Text;

                ORProcessOperators optr = tsm.UsedOperator;

                switch (Convert.ToInt32(optr))
                {
                    case 0:
                        if (tsm.ORNumberFrom.Trim().Length == 8)
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeEightNone;
                        }
                        else// seven digit OR
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeSevenNone;
                        }
                        break;
                    case 1: //addition
                        if (tsm.ORNumberFrom.Trim().Length == 8)
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeEightAddition;
                        }
                        else// seven digit OR
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeSevenAddition;
                        }
                        da.SelectCommand.Parameters.AddWithValue("@ADDEND", tsm.Addend);
                        break;
                    case 2: //subtraction
                        if (tsm.ORNumberFrom.Trim().Length == 8)
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeEightSubtraction;
                        }
                        else// seven digit OR
                        {
                            da.SelectCommand.CommandText = HomeQueries.sqlListNewPrefixOfORRangeSevenSubtraction;
                        }
                        da.SelectCommand.Parameters.AddWithValue("@SUBTRACT", tsm.Subtrahend);
                        break;
                }
                

                da.SelectCommand.Parameters.AddWithValue("@ORFROM", tsm.ORNumberFrom.Trim());
                da.SelectCommand.Parameters.AddWithValue("@ORTO", tsm.ORNumberTo.Trim());
                da.SelectCommand.Parameters.AddWithValue("@NEWPREFIX", tsm.NewPrefix.Trim());

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstthm.Add(new TransactionHeaderModel
                            {
                                ORNumber = dr["ornumber"].ToString(),
                                Payee = dr["payee"].ToString(),
                                AccountNumber = dr["consumerid"].ToString(),
                                NewORNumber = dr["newor"].ToString(),
                                TransactionDate = dr["trxdate"].ToString()
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lstthm = null;
                }
            }
            return lstthm;
        }

        public List<TransactionHeaderModel> ListOfORInAddition(TransactionSearchModel tsm)
        {
            List<TransactionHeaderModel> lstthm = new List<TransactionHeaderModel>();
            using(SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.CommandType = CommandType.Text;
                if (tsm.ORNumberFrom.Trim().Length == 8)
                {
                    da.SelectCommand.CommandText = HomeQueries.sqlListOfORInEightAddition;
                }
                else// seven digit OR
                {
                    da.SelectCommand.CommandText = HomeQueries.sqlListOfORInSevenAddition;
                }
                
                da.SelectCommand.Parameters.AddWithValue("@ORFROM", tsm.ORNumberFrom.Trim());
                da.SelectCommand.Parameters.AddWithValue("@ORTO", tsm.ORNumberTo.Trim());
                da.SelectCommand.Parameters.AddWithValue("@ADDEND", tsm.Addend);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            lstthm.Add(new TransactionHeaderModel
                            {
                                ORNumber = dr["ornumber"].ToString(),
                                Payee = dr["payee"].ToString(),
                                AccountNumber = dr["consumerid"].ToString(),
                                NewORNumber = dr["newor"].ToString(),
                                TransactionDate = dr["trxdate"].ToString()
                            });
                        }
                        
                    }
                }
                catch (Exception)
                {
                    lstthm = null;
                }
            }
            return lstthm;
        }

        public List<TransactionHeaderModel> ListOfORInSubtraction(TransactionSearchModel tsm)
        {
            List<TransactionHeaderModel> lstthm = new List<TransactionHeaderModel>();
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.CommandType = CommandType.Text;
                if (tsm.ORNumberFrom.Trim().Length == 8)
                {
                    da.SelectCommand.CommandText = HomeQueries.sqlListOfORInEightSubtraction;
                }
                else// seven digit OR
                {
                    da.SelectCommand.CommandText = HomeQueries.sqlListOfORInSevenSubtraction;
                }

                da.SelectCommand.Parameters.AddWithValue("@ORFROM", tsm.ORNumberFrom.Trim());
                da.SelectCommand.Parameters.AddWithValue("@ORTO", tsm.ORNumberTo.Trim());
                da.SelectCommand.Parameters.AddWithValue("@SUBTRACT", tsm.Subtrahend);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstthm.Add(new TransactionHeaderModel
                            {
                                ORNumber = dr["ornumber"].ToString(),
                                Payee = dr["payee"].ToString(),
                                AccountNumber = dr["consumerid"].ToString(),
                                NewORNumber = dr["newor"].ToString(),
                                TransactionDate = dr["trxdate"].ToString()
                            });
                        }

                    }
                }
                catch (Exception)
                {
                    lstthm = null;
                }
            }
            return lstthm;
        }

        public VMNewORByRange ProcessNewORByRangeAdd(string ORFrom, string ORTo, int Additional)
        {
            VMNewORByRange vmnobr = new VMNewORByRange();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                //SqlTransaction trans = cmd.Connection.BeginTransaction();
                //cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeEightDigAdd;
                }
                else// seven digit OR
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeSevenDigAdd;
                }
                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@ADD", Additional);
                try
                {
                    cmd.ExecuteNonQuery();
                    vmnobr.isSuccess = true;
                    vmnobr.errMessage = "Official Receipt Number(s) are successfully updated.";
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    vmnobr.isSuccess = false;
                    vmnobr.errMessage = String.Format("An error occured. \n {0}", ex.Message);
                }
            }
            return vmnobr;
        }

        public VMNewORByRange ProcessNewORByRangeDiff(string ORFrom, string ORTo, int Difference)
        {
            VMNewORByRange vmnobr = new VMNewORByRange();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                //SqlTransaction trans = cmd.Connection.BeginTransaction();
                //cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeEightDigSubtract;
                }
                else// seven digit OR
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeSevenDigSubtract;
                }
                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                try
                {
                    cmd.ExecuteNonQuery();
                    vmnobr.isSuccess = true;
                    vmnobr.errMessage = "Official Receipt Number(s) are successfully updated.";
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    vmnobr.isSuccess = false;
                    vmnobr.errMessage = String.Format("An error occured. \n {0}", ex.Message);

                }
            }
            return vmnobr;
        }

        public VMNewPrefixOfORRange ProcessNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0)
        {
            VMNewPrefixOfORRange vmnpor = new VMNewPrefixOfORRange();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                SqlTransaction trans = cmd.Connection.BeginTransaction();
                cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    if (Difference == 0 && Additional == 0)
                    {
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightNone;
                    }
                    else if (Difference > 0 && Additional == 0)
                    {
                        //difference OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightSubtraction;
                        cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                    }
                    else
                    {
                        //addition OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightAddition;
                        cmd.Parameters.AddWithValue("@ADDEND", Additional);
                    }

                }
                else// seven digit OR
                {
                    if (Difference == 0 && Additional == 0)
                    {
                        //none operator
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightNone;
                    }
                    else if (Difference > 0 && Additional == 0)
                    {
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeSevenSubtraction;
                        cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                    }
                    else
                    {
                        //addition OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeSevenAddition;
                        cmd.Parameters.AddWithValue("@ADDEND", Additional);
                    }
                }

                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@NEWPREFIX", NewPrefix.Trim());

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    vmnpor.isSuccess = true;
                    vmnpor.errMessage = "Official Receipt Number(s) are successfully updated.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    vmnpor.isSuccess = false;
                    vmnpor.errMessage = String.Format("An error occured. \n{0}.", ex.Message);
                }
            }
            return vmnpor;
        }

        public bool SetNewORByRangeAdd(string ORFrom, string ORTo, int Additional)
        {
            bool result = false;
            using (SqlCommand cmd = new SqlCommand())
             {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                //SqlTransaction trans = cmd.Connection.BeginTransaction();
                //cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeEightDigAdd;
                }
                else// seven digit OR
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeSevenDigAdd;
                }
                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@ADD", Additional);
                try
                {
                    cmd.ExecuteNonQuery();
                    //trans.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    result = false;
                }
            }
            return result;
        }

        public bool SetNewORByRangeDiff(string ORFrom, string ORTo, int Difference)
        {

            bool result = false;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                //SqlTransaction trans = cmd.Connection.BeginTransaction();
                //cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeEightDigSubtract;
                }
                else// seven digit OR
                {
                    cmd.CommandText = HomeQueries.sqlSetNewORByRangeSevenDigSubtract;
                }
                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                try
                {
                    cmd.ExecuteNonQuery();
                    //trans.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    result = false;
                  
                }
            }
            return result;
        }

        public bool SetNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0)
        {
            bool result = false;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(this.ConnectionString);
                cmd.Connection.Open();

                SqlTransaction trans = cmd.Connection.BeginTransaction();
                cmd.Transaction = trans;

                cmd.CommandType = CommandType.Text;
                if (ORFrom.Trim().Length == 8)
                {
                    if(Difference==0 && Additional == 0)
                    {
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightNone;
                    }else if(Difference > 0 && Additional == 0)
                    {
                        //difference OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightSubtraction;
                        cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                    }
                    else
                    {
                        //addition OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightAddition;
                        cmd.Parameters.AddWithValue("@ADDEND", Additional);
                    }
                    
                }
                else// seven digit OR
                {
                    if (Difference == 0 && Additional == 0)
                    {
                        //none operator
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeEightNone;
                    }
                    else if (Difference > 0 && Additional == 0)
                    {
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeSevenSubtraction;
                        cmd.Parameters.AddWithValue("@SUBTRACT", Difference);
                    }
                    else
                    {
                        //addition OR
                        cmd.CommandText = HomeQueries.sqlSetNewPrefixOfORRangeSevenAddition;
                        cmd.Parameters.AddWithValue("@ADDEND", Additional);
                    }
                }

                cmd.Parameters.AddWithValue("@ORFROM", ORFrom.Trim());
                cmd.Parameters.AddWithValue("@ORTO", ORTo.Trim());
                cmd.Parameters.AddWithValue("@NEWPREFIX", NewPrefix.Trim());

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result = false;
                }
            }
            return result;
        }

        
    }
}