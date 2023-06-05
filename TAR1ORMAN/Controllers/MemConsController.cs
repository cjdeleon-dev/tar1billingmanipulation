using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.ChangeStatusService;

namespace TAR1ORMAN.Controllers
{
    public class MemConsController : Controller
    {
        IChangeStatusService icss;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MSERVE,SYSADMIN,TELLER")]
        // GET: MemCons
        public ActionResult MemberConsumer()
        {
            ViewBag.IsRecOfcr = User.IsInRole("MSERVE").ToString();
            ViewBag.IsTeller = User.IsInRole("TELLER").ToString();

            return View();
        }

        public JsonResult GetAccountDetails(string accountNum)
        {
            MemberConsumerModel mcm = new MemberConsumerModel();
            mcm = getAccountDetailsByAcctNum(accountNum);
            return Json(mcm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllTypes()
        {
            return Json(getAllTypes(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAreas()
        {
            return Json(getAllAreas(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllOffices()
        {
            return Json(getAllOffices(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStatus(string exceptstat)
        {
            return Json(getAllStatusExcept(exceptstat), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetStatusOfConsumerById(ConsumerStatusLogModel cslm)
        {
            icss = new ChangeStatusService();

            string userid = User.Identity.Name;
            cslm.EntryUser = userid;

            ProcessResultModel prm = icss.SetStatusOfConsumerId(cslm);

            return Json(prm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadStatusLogData(string accountno)
        {
            icss = new ChangeStatusService();

            List<ConsumerStatusLogModel> data;

            data = icss.GetStatusLogByConsumerid(accountno);

            var jsonResult = Json(new { data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult UpdateMemConsProfile(MemConsProfileModel mcpm)
        {
            mcpm.UpdatedBy = User.Identity.Name;
            return Json(updateMemConsProfile(mcpm), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewAccountBalance(string accountno)
        {
            List<MemConsBalanceModel> lstmcbm = new List<MemConsBalanceModel>();
            lstmcbm = viewAccountBalanceTable(accountno);
            if (lstmcbm != null)
                return Json(new { data = lstmcbm }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }



        //functions and procedures
        private MemberConsumerModel getAccountDetailsByAcctNum(string acctnum)
        {
            MemberConsumerModel mcm = new MemberConsumerModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select consumerid,cons.consumertypeid,rtrim(typ.description)[consumertype]," +
                                  "consclass,rtrim(name)[name],rtrim(address)[address],rtrim(poleid)[poleid]," +
                                  "cons.statusid,rtrim(stat.description)[status],isnull(memberid,'')memberid,memberdate,bookno," +
                                  "seqno,cons.areaid,area.description[area],cons.subofficeid[officeid]," +
                                  "ofc.description[office],flatrate,flatdemand,coreloss,kvaload,tsfrental," +
                                  "mtrmultiplier,trfcount,mtrserialno,mtrsealno,mtrsidesealno,metertype," +
                                  "mtrbrand,dateinstalled,prvmtrserialno,mtrampere,mtrdial,scflag,clrsr,dateclaimburial " +
                                  "from arsconsumer cons inner join arstype typ " +
                                  "on cons.consumertypeid = typ.consumertypeid " +
                                  "inner join arsstatus stat " +
                                  "on cons.statusid = stat.statusid " +
                                  "inner join arsarea area " +
                                  "on cons.areaid = area.areaid " +
                                  "inner join arssuboffice ofc " +
                                  "on cons.subofficeid = ofc.subofficeid " +
                                  "where consumerid = @consumerid";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            mcm.ConsumerId = acctnum;
                            mcm.ConsumerTypeId = rdr["consumertypeid"].ToString();
                            mcm.ConsumerType = rdr["consumertype"].ToString();
                            mcm.ConsumerClass = rdr["consclass"].ToString();
                            mcm.Name = rdr["name"].ToString();
                            mcm.Address = rdr["address"].ToString();
                            mcm.PoleId = rdr["poleid"].ToString();
                            mcm.StatusId = rdr["statusid"].ToString();
                            mcm.Status = rdr["status"].ToString();
                            mcm.MemberOR = rdr["memberid"].ToString();
                            if (rdr["memberdate"].ToString() != "")
                                mcm.MemberDate = Convert.ToDateTime(rdr["memberdate"].ToString()).ToString("yyyy-MM-dd");
                            else
                                mcm.MemberDate = "";
                            mcm.BookNo = rdr["bookno"].ToString();
                            mcm.SequenceNo = rdr["seqno"].ToString();
                            mcm.AreaId = rdr["areaid"].ToString();
                            mcm.Area = rdr["area"].ToString();
                            mcm.OfficeId = rdr["officeid"].ToString();
                            mcm.Office = rdr["office"].ToString();
                            if (rdr["dateclaimburial"].ToString() != "")
                                mcm.ClaimedBurialDate = Convert.ToDateTime(rdr["dateclaimburial"].ToString()).ToString("yyyy-MM-dd");
                            else
                                mcm.ClaimedBurialDate = "";
                            mcm.FlatRate = Convert.ToDouble(rdr["flatrate"]);
                            mcm.FlatDemand = Convert.ToDouble(rdr["flatdemand"]);
                            mcm.Coreloss = Convert.ToDouble(rdr["coreloss"]);
                            mcm.KVALoad = Convert.ToDouble(rdr["kvaload"]);
                            mcm.TSFRental = Convert.ToDouble(rdr["tsfrental"]);
                            mcm.Multiplier = Convert.ToDouble(rdr["mtrmultiplier"]);
                            mcm.TSFCount = Convert.ToInt32(rdr["trfcount"]);
                            mcm.MeterSerialNo = rdr["mtrserialno"].ToString();
                            mcm.MeterSealNo = rdr["mtrsealno"].ToString();
                            mcm.MeterSideSealNo = rdr["mtrsidesealno"].ToString();
                            mcm.MeterType = rdr["metertype"].ToString();
                            mcm.MeterBrand = rdr["mtrbrand"].ToString();
                            if (rdr["dateinstalled"].ToString() != "")
                                mcm.DateInstalled = Convert.ToDateTime(rdr["dateinstalled"].ToString()).ToString("yyyy-MM-dd");
                            else
                                mcm.DateInstalled = "";
                            mcm.PrevMeterSerialNo = rdr["prvmtrserialno"].ToString();
                            mcm.MeterAmp = Convert.ToInt32(rdr["mtrampere"]);
                            mcm.MeterDial = Convert.ToInt32(rdr["mtrdial"]);
                            mcm.SCFlag = rdr["scflag"].ToString();
                            mcm.CLRSR = rdr["clrsr"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    mcm = null;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return mcm;
        }

        private List<ConsumerTypeModel> getAllTypes()
        {
            List<ConsumerTypeModel> lstctm = new List<ConsumerTypeModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select consumertypeid[id],description[type] from arstype;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstctm.Add(new ConsumerTypeModel
                            {
                                Id = dr["id"].ToString(),
                                ConsumerType = dr["type"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstctm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstctm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstctm;
        }

        private List<ArsAreaModel> getAllAreas()
        {
            List<ArsAreaModel> lstam = new List<ArsAreaModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select areaid [id],description [area] from arsarea;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstam.Add(new ArsAreaModel
                            {
                                Id = dr["id"].ToString(),
                                Area = dr["area"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstam = null;
                    }
                }
                catch (Exception ex)
                {
                    lstam = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstam;
        }

        private List<ArsSubOfficeModel> getAllOffices()
        {
            List<ArsSubOfficeModel> lstom = new List<ArsSubOfficeModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select subofficeid[id],description[office] from arssuboffice;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstom.Add(new ArsSubOfficeModel
                            {
                                Id = dr["id"].ToString(),
                                Office = dr["office"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstom = null;
                    }
                }
                catch (Exception ex)
                {
                    lstom = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstom;
        }

        private List<StatusModel> getAllStatusExcept(string exceptStat)
        {
            List<StatusModel> lstsm = new List<StatusModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select statusid, description from arsstatus where statusid<>@exceptstat and statusid not in ('F','T');";

                da.SelectCommand.Parameters.AddWithValue("@exceptstat", exceptStat);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstsm.Add(new StatusModel
                            {
                                StatusId = dr["statusid"].ToString(),
                                Description = dr["description"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstsm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstsm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstsm;
        }

        private bool updateMemConsProfile(MemConsProfileModel mm)
        {
            bool result = true;

            SqlTransaction trans;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                trans = con.BeginTransaction();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "update arsconsumer " +
                                  "set address=@address,poleid=@poleid,consumertypeid=@consumertypeid,memberid=@memberid,memberdate=@memberdate," +
                                  "bookno=@bookno,seqno=@seqno,areaid=@areaid,subofficeid=@subofficeid,isclaimburial=@isclaimburial," +
                                  "dateclaimburial=@dateclaimburial,updatedby=@updatedby,lastupdated=getdate() " +
                                  "where consumerid=@consumerid;";
                cmd.Transaction = trans;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@address", mm.AccountAdd);
                if(mm.AccountPoleId!=null)
                    cmd.Parameters.AddWithValue("@poleid", mm.AccountPoleId);
                else
                    cmd.Parameters.AddWithValue("@poleid", DBNull.Value);
                cmd.Parameters.AddWithValue("@consumertypeid", mm.AccountTypeId);
                if(mm.MemberOR != null)
                {
                    if(mm.MemberOR.ToString().Trim()!=string.Empty || mm.MemberOR.ToString().Trim()!="")
                        cmd.Parameters.AddWithValue("@memberid", mm.MemberOR.Trim());
                    else
                        cmd.Parameters.AddWithValue("@memberid", "");
                }    
                else
                    cmd.Parameters.AddWithValue("@memberid", "");
                if (mm.MemberORDate!=null)
                    cmd.Parameters.AddWithValue("@memberdate", mm.MemberORDate);
                else
                    cmd.Parameters.AddWithValue("@memberdate", DBNull.Value);
                cmd.Parameters.AddWithValue("@bookno", mm.BookNo);
                cmd.Parameters.AddWithValue("@seqno", mm.SeqNo);
                cmd.Parameters.AddWithValue("@areaid", mm.AreaId);
                cmd.Parameters.AddWithValue("@subofficeid", mm.OfficeId);
                cmd.Parameters.AddWithValue("@isclaimburial", mm.IsClaimedBurial);
                if (mm.ClaimedBurialDate != null)
                    cmd.Parameters.AddWithValue("@dateclaimburial", mm.ClaimedBurialDate);
                else
                    cmd.Parameters.AddWithValue("@dateclaimburial", DBNull.Value);
                cmd.Parameters.AddWithValue("@updatedby", mm.UpdatedBy);
                cmd.Parameters.AddWithValue("@consumerid", mm.AccountNo);

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
                finally
                {
                    trans.Dispose();
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        private List<MemConsBalanceModel> viewAccountBalanceTable(string accountnum)
        {
            List<MemConsBalanceModel> lstmcbm = new List<MemConsBalanceModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_GetDuePerMonthByConsumerId";

                da.SelectCommand.Parameters.AddWithValue("@consumerid", accountnum);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstmcbm.Add(new MemConsBalanceModel
                            {
                                AccountNumber = dr["consumerid"].ToString(),
                                BillPeriod = dr["billperiod"].ToString(),
                                TrxBalance = Convert.ToDouble(dr["trxbalance"]),
                                VATBalance = Convert.ToDouble(dr["vatbalance"]),
                                Surcharge = Convert.ToDouble(dr["surcharge"]),
                                TotalAmount = Convert.ToDouble(dr["rowtotal"]),
                                Months = Convert.ToInt32(dr["months"]),
                                PayAmount = Convert.ToDouble(dr["payamt"])
                            });
                        }
                    }
                    else
                    {
                        lstmcbm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstmcbm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstmcbm;
        }
    }
}