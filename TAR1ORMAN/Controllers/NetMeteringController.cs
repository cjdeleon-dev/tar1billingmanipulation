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
            ViewBag.AccountNo = _selactno;
            
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
                cmd.CommandText = "select rtrim(name)[name], rtrim(address)[address], rtrim(description)[status] " +
                                  "from arsconsumer cons inner join arstype typ " +
                                  "on cons.consumertypeid=typ.consumertypeid " +
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
    }
}