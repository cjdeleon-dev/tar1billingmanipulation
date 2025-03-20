using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TAR1ORMAN.Controllers
{
    public class NetMeteringController : Controller
    {
        private static string _selactno = string.Empty;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,SYSADMIN")]
        // GET: NetMetering
        public ActionResult NetMeteringList()
        {
            ViewBag.UserId = User.Identity.Name;
            if (User.IsInRole("SYSADMIN"))
            {
                ViewBag.Message = "ADMIN";
            }
            else
            {
                ViewBag.Message = "NONADMIN";
            }

            return View();
        }

        public JsonResult LoadData()
        {
            List<NetMeteringModel> lstnetmtr = new List<NetMeteringModel>();
            lstnetmtr = loadData();
            return Json(new { data = lstnetmtr }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SetAccountLedger(string accountNo)
        {
            _selactno = accountNo;
            return RedirectToAction("ViewConsumerLedger");
        }

        public ActionResult ViewConsumerLedger()
        {
            //ViewBag.AccountNo = _selactno;
            NMSelAcctNoModel sanm = new NMSelAcctNoModel();
            sanm.SelectedAcctNo = _selactno;
            _selactno = string.Empty;
            
            if (User.IsInRole("SYSADMIN"))
            {
                ViewBag.Message = "ADMIN";
            }
            else
            {
                ViewBag.Message = "NONADMIN";
            }
            return View(sanm);
        }

        public JsonResult GetAccountDetails(string accountNo)
        {
            return Json(new { data = getAccountDetails(accountNo) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadLedgerByAccountNo(string actno)
        {
            var jsonResult = Json(new { data = loadLedgerByAccountNo(actno) }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult GetCurrentBillPeriod()
        {
            return Json(new { data = getCurrentBillPeriod() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDemandByAccountNo(string acctno)
        {
            return Json(new { data = getDemandByAccountNo(acctno) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrevReadingByAccountNo(string acctno)
        {
            NetMeteringPrevReadModel nmprm = new NetMeteringPrevReadModel();
            nmprm = getPrevReadingByAccountNo(acctno);

            return Json(new { data=nmprm }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessBill(NetMeteringInReadModel nmrm)
        {
            return Json(new { data = processNetMeteringBill(nmrm) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewNetMeteringBill(NetMeteringPostBillModel nmpbm)
        {
            nmpbm.EntryUser = User.Identity.Name;
            return Json(new { data = saveNewNetMeteringBill(nmpbm) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RebuildByAccountNo(string acctno)
        {
            return Json(new { data = rebuildByAccountNo(acctno) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddAccountAsNetMetering(ConsumerModel cm)
        {
            return Json(addAccountAsNetMetering(cm.AccountNo), JsonRequestBehavior.AllowGet);
        }

        //FUNCTIONS AND PROCEDURES
        private List<NetMeteringModel> loadData()
        {
            List<NetMeteringModel> lst = new List<NetMeteringModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select consumerid,rtrim(name)[name],rtrim(address)[address],rtrim(typ.description) [type]," +
                                               "stat.description[status],mtrserialno " +
                                               "from arsconsumer cons inner join arstype typ " +
                                               "on cons.consumertypeid = typ.consumertypeid " +
                                               "inner join arsstatus stat " +
                                               "on cons.statusid = stat.statusid " +
                                               "where isnetmtr = 1";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lst.Add(new NetMeteringModel {
                                AccountNo = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                Type = dr["type"].ToString(),
                                Status = dr["status"].ToString(),
                                MeterNo = dr["mtrserialno"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
                catch (Exception ex)
                {
                    lst = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lst;
        }

        private List<ConsumerLedgerModel> loadLedgerByAccountNo(string actno)
        {
            List<ConsumerLedgerModel> lst = new List<ConsumerLedgerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "cp_getledger";

                da.SelectCommand.Parameters.AddWithValue("@consumerid", actno);

                DataTable dt = new DataTable();

                double lntrxamount = 0;
                double lnvatamount = 0;

                try
                {
                    da.Fill(dt);

                    //get the number of month arrear(s)
                    var query = (from result1
                                     in dt.AsEnumerable()
                                 where result1.Field<Decimal>("trxbalance") > 0
                                 select result1);
                    int moncnt = query.Count();

                    //datatable manipulation
                    DataColumn ncol = new DataColumn("isBal", typeof(bool));
                    ncol.DefaultValue = false;

                    dt.Columns.Add(ncol);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i=0; i<=dt.Rows.Count-1; i++)
                        {
                            //to determine each billing period with trx balance as a reference to change font color in the jquery datatable.
                            if (Convert.ToDouble(dt.Rows[i]["trxbalance"]) > 0 && dt.Rows[i]["trxid"].ToString().Equals("EB"))
                            {
                                dt.Rows[i]["isBal"] = true;
                            }

                            if (dt.Rows[i]["trxid"].ToString().Equals("EB") || dt.Rows[i]["trxid"].ToString().Equals("DM"))
                            {
                                lntrxamount = Math.Round(lntrxamount + Convert.ToDouble(dt.Rows[i]["trxamount"]),2);
                                lnvatamount = Math.Round(lnvatamount + Convert.ToDouble(dt.Rows[i]["vat"]),2);
                                dt.Rows[i]["trxbalance"] = lntrxamount;
                                dt.Rows[i]["vatbalance"] = lnvatamount;
                            }
                            else if (!dt.Rows[i]["trxid"].ToString().Equals("EB") && !dt.Rows[i]["trxid"].ToString().Equals("DM"))
                            {
                                lntrxamount = Math.Round(lntrxamount - Convert.ToDouble(dt.Rows[i]["trxamount"]),2);
                                lnvatamount = Math.Round(lnvatamount - Convert.ToDouble(dt.Rows[i]["vat"]),2);
                                dt.Rows[i]["trxbalance"] = lntrxamount;
                                dt.Rows[i]["vatbalance"] = lnvatamount;
                            }       
                        }
                        dt.AcceptChanges();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lst.Add(new ConsumerLedgerModel
                            {
                                TrxSeqId = Convert.ToInt32(dr["trxseqid"]),
                                TrxDate = Convert.ToDateTime(dr["trxdate"]).ToString("yyyy-MM-dd"),
                                Trx = dr["trxid"].ToString(),
                                Period = dr["trxid"].ToString() != "EB" ? dr["reference"].ToString()
                                : dr["description"].ToString(),
                                Prev = Convert.ToInt32(dr["prevreading"]),
                                Curr = Convert.ToInt32(dr["currreading"]),
                                KWh = Convert.ToInt32(dr["energyused"]),
                                DMU = Convert.ToDouble(dr["demandused"]),
                                TrxAmount = Convert.ToDouble(dr["trxamount"]),
                                TrxBalance = Convert.ToDouble(dr["trxbalance"]),
                                VAT = Convert.ToDouble(dr["vat"]),
                                VATBalance = Convert.ToDouble(dr["vatbalance"]),
                                TotalTrxBalance = lntrxamount,
                                TotalVatBalance = lnvatamount,
                                Months = Convert.ToInt32(moncnt),
                                isBalance = Convert.ToBoolean(dr["isBal"])
                            });
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
                catch (Exception ex)
                {
                    lst = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lst;
        }

        private ConsumerModel getAccountDetails(string actno)
        {
            ConsumerModel cm = new ConsumerModel();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select rtrim(name)[name], rtrim(address)[address],poleid,mtrserialno,rtrim(description)[status] " +
                                  "from arsconsumer cons inner join arsstatus stat " +
                                  "on cons.statusid=stat.statusid " +
                                  "where cons.consumerid=@consumerid;";

                cmd.Parameters.AddWithValue("@consumerid", actno);

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cm.AccountNo = actno;
                            cm.AccountName = dr["name"].ToString();
                            cm.Address = dr["address"].ToString();
                            cm.Status = dr["status"].ToString();
                            cm.PoleId = dr["poleid"].ToString();
                            cm.MeterNo = dr["mtrserialno"].ToString();
                        }
                    }
                }
                catch (Exception)
                {
                    cm = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return cm;
        }

        private string getCurrentBillPeriod()
        {
            string bp = string.Empty;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select top 1 billperiod " +
                                  "from arsbillperiod " +
                                  "order by billperiod desc;";

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            bp = dr["billperiod"].ToString();
                        }
                    }
                }
                catch (Exception)
                {
                    bp = string.Empty;
                }
                finally
                {
                    con.Close();
                }
            }

            return bp;
        }

        private decimal getDemandByAccountNo(string actno)
        {
            decimal demand = 0;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select flatdemand " +
                                  "from arsconsumer " +
                                  "where consumerid=@consumerid;";

                cmd.Parameters.AddWithValue("@consumerid", actno);

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            demand = Convert.ToDecimal(dr["flatdemand"]);
                        }
                    }
                }
                catch (Exception)
                {
                    demand = 0;
                }
                finally
                {
                    con.Close();
                }
            }

            return demand;
        }

        private NetMeteringPrevReadModel getPrevReadingByAccountNo(string actno)
        {
            NetMeteringPrevReadModel nm = new NetMeteringPrevReadModel();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "declare @prevbp as varchar(6);" +
                                  "select @prevbp = max(billperiod) from tbl_netmeteringread where consumerid=@consumerid;" +
                                  "select @prevbp [billperiod],currimp[previmp],currexp[prevexp],currrec[prevrec] " +
                                  "from tbl_netmeteringread " +
                                  "where consumerid = @consumerid " +
                                  "and billperiod=@prevbp;";

                cmd.Parameters.AddWithValue("@consumerid", actno);

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            nm.BillPeriod = dr["billperiod"].ToString();
                            nm.PrevImp = Convert.ToInt32(dr["previmp"]);
                            nm.PrevExp = Convert.ToInt32(dr["prevexp"]);
                            nm.PrevRec = Convert.ToInt32(dr["prevrec"]);
                        }
                    }
                }
                catch (Exception)
                {
                    nm = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return nm;
        }

        private NetMeteringBillModel processNetMeteringBill(NetMeteringInReadModel nmirm)
        {
            NetMeteringBillModel nmbm = new NetMeteringBillModel();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_computeBillByNetMetering";

                cmd.Parameters.AddWithValue("@consumerid", nmirm.ConsumerId);
                cmd.Parameters.AddWithValue("@billperiod", nmirm.BillPeriod);
                cmd.Parameters.AddWithValue("@energyused", nmirm.NetImport);
                cmd.Parameters.AddWithValue("@netexport", nmirm.NetExport);
                cmd.Parameters.AddWithValue("@demand", nmirm.ActualDemand);

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            nmbm.EnergyAmount = Convert.ToDecimal(dr["EnergyAmount"]);
                            nmbm.DemandAmount = Convert.ToDecimal(dr["DemandAmount"]);
                            nmbm.BillAmount = Convert.ToDecimal(dr["BillAmount"]);
                            nmbm.VATAmount = Convert.ToDecimal(dr["VATAmount"]);
                            nmbm.TotalAmount = Convert.ToDecimal(dr["Total"]);
                            nmbm.NetBillAmount = Convert.ToDecimal(dr["NetBillAmount"]);
                            nmbm.TotalCurrentBill = Convert.ToDecimal(dr["TotalCurrentBill"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    nmbm = null;
                }
                finally
                {
                    con.Close();
                }

                return nmbm;
            }
        }

        private bool saveNewNetMeteringBill(NetMeteringPostBillModel nmbm)
        {
            bool res = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
            SqlTransaction trans;

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                trans = con.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_postnetmeteringbill";

                cmd.Parameters.AddWithValue("@consumerid", nmbm.ConsumerId);
                cmd.Parameters.AddWithValue("@previmp", nmbm.PrevImp);
                cmd.Parameters.AddWithValue("@currimp", nmbm.CurrImp);
                cmd.Parameters.AddWithValue("@energyused", nmbm.NetImp);
                cmd.Parameters.AddWithValue("@prevexp", nmbm.PrevExp);
                cmd.Parameters.AddWithValue("@currexp", nmbm.CurrExp);
                cmd.Parameters.AddWithValue("@netexport", nmbm.NetExp);
                cmd.Parameters.AddWithValue("@prevrec", nmbm.PrevRec);
                cmd.Parameters.AddWithValue("@currrec", nmbm.CurrRec);
                cmd.Parameters.AddWithValue("@demand", nmbm.Demand);
                cmd.Parameters.AddWithValue("@trxdate", nmbm.TrxDate);
                cmd.Parameters.AddWithValue("@billperiod", nmbm.BillPeriod);
                cmd.Parameters.AddWithValue("@entryuser", nmbm.EntryUser);

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    res = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    res = false;
                }
                finally
                {
                    trans.Dispose();
                    con.Close();
                }
            }
            return res;
        }

        private bool rebuildByAccountNo(string actno)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "cp_autoapply";

                cmd.Parameters.AddWithValue("@consumerid",actno);

                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    con.Close();
                }
            }

            return result;
        }


        private bool addAccountAsNetMetering(string consumerid)
        {
            bool res = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
            SqlTransaction trans;

            string userid = User.Identity.Name;

            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                trans = con.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update arsconsumer set isnetmtr=1 where consumerid=@consumerid; " +
                                  "insert into tblAuditTrail values(3, 'arsconsumer', 'Update account as new net metering consumer.', @userid, getdate());";

                cmd.Parameters.AddWithValue("@consumerid", consumerid);
                cmd.Parameters.AddWithValue("@userid", userid);

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    res = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    res = false;
                }
                finally
                {
                    trans.Dispose();
                    con.Close();
                }
            }
            return res;
        }
    }
}