using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class TORController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,SYSADMIN,TELLER")]
        // GET: TOR
        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.Name;
            return View();
        }

        public JsonResult GetPaymentByOrnumber(string ornum)
        {
            return Json(getPaymentByORNumber(ornum),JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTORHeaderByORNumber(string ornum)
        {
            return Json(getPaymentHeaderByORNumber(ornum),JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckIfHasDetails(string ornum)
        {
            List<PaymentDetailModel> details = new List<PaymentDetailModel>();
            details = getPaymentDetailsByORNumber(ornum);
            if(details.Count>0)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTORDetailsByORNumber(string ornum)
        {
            return Json(getPaymentDetailsByORNumber(ornum), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTORAppliesByORNumber(string ornum)
        {
            return Json(getPaymentAppliesByORNumber(ornum), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetComparisonPaymentToMasterByORNumber(string ornum)
        {
            return Json(getComparisonMasterPaymentConsumerIdByORNumber(ornum), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePaymentConsumerIdByORNumber(string ornum, string newconsumerid)
        {
            return Json(new { data = isUpdateSuccess(ornum, newconsumerid, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }




        private PaymentViewModel getPaymentByORNumber(string ornumber)
        {
            PaymentViewModel vm = new PaymentViewModel();

            vm.PaymentHeader = getPaymentHeaderByORNumber(ornumber);
            vm.PaymentDetails = getPaymentDetailsByORNumber(ornumber);
            vm.PaymentApplies = getPaymentAppliesByORNumber(ornumber);

            return vm;
        }

        private PaymentHeaderModel getPaymentHeaderByORNumber(string ornumber)
        {
            PaymentHeaderModel phm = new PaymentHeaderModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select rtrim(hdr.ornumber)ornumber,hdr.consumerid,rtrim(hdr.address)address,rtrim(trx.description)[modeofpayment], " +
                                  "rtrim(rev.description)[office],rtrim(payee)payee,ISNULL(tin,'')tin,CONVERT(varchar, trxdate, 101)[trxdate],ISNULL(chknumber,'')[chknumber]," +
                                  "ISNULL(chkname,'')[chkname],ISNULL(bank,'')[bank],hdr.ttlamount,tendered,tendered-hdr.ttlamount[change]," +
                                  "case when hdr.statusid='X' then 'CANCELED' else '' end status,rtrim(usr.name)[entryuser]," +
                                  "entrydate,typ.description[consumertype],stat.description[consumerstatus] " +
                                  "from arspaytrxhdr hdr inner join arsrevcenter rev " +
                                  "on hdr.revcenterid=rev.revcenterid " +
                                  "inner join arstrx trx " +
                                  "on hdr.trxid=trx.trxid " +
                                  "inner join secuser usr " +
                                  "on hdr.entryuser=usr.userid " +
                                  "left join arsconsumer cons " +
                                  "on hdr.consumerid=cons.consumerid " +
                                  "left join arstype typ " +
                                  "on cons.consumertypeid=typ.consumertypeid " +
                                  "left join arsstatus stat " +
                                  "on cons.statusid=stat.statusid " +
                                  "where hdr.ornumber=@ornumber";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ornumber", ornumber);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            phm.ORNumber = rdr["ornumber"].ToString();
                            phm.ConsumerId = rdr["consumerid"].ToString();
                            phm.ConsumerType = rdr["consumertype"].ToString().ToUpper();
                            phm.ConsumerStatus = rdr["consumerstatus"].ToString().ToUpper();
                            phm.Payee = rdr["payee"].ToString();
                            phm.Address = rdr["address"].ToString();
                            phm.TrxDate = Convert.ToDateTime(rdr["trxdate"]).ToString("yyyy-MM-dd");
                            phm.ModeOfPayment = rdr["modeofpayment"].ToString().ToUpper();
                            phm.Office = rdr["office"].ToString().ToUpper();
                            phm.TIN = rdr["tin"].ToString();
                            phm.CheckNumber = rdr["chknumber"].ToString();
                            phm.CheckName = rdr["chkname"].ToString();
                            phm.Bank = rdr["bank"].ToString();
                            phm.TotalAmount = Convert.ToDouble(rdr["ttlamount"]);
                            phm.Tendered = Convert.ToDouble(rdr["tendered"]);
                            phm.Change = Convert.ToDouble(rdr["change"]);
                            phm.TrxStatus = rdr["status"].ToString();
                            phm.EntryUser = rdr["entryuser"].ToString();
                            phm.EntryDate = rdr["entrydate"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    phm = null;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return phm;
        }

        private List<PaymentDetailModel> getPaymentDetailsByORNumber(string ornumber)
        {
            List<PaymentDetailModel> lpdm = new List<PaymentDetailModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getPaymentDetailsByORNumber";

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@ornumber", ornumber);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lpdm.Add(new PaymentDetailModel
                            {
                                TrxId = dr["trxid"].ToString(),
                                TrxDesc = dr["trxdesc"].ToString(),
                                TrxAmount = Convert.ToDouble(dr["trxamount"]),
                                VAT = Convert.ToDouble(dr["vat"]),
                                Amount = Convert.ToDouble(dr["amount"])
                            });

                        }
                    }
                    else
                    {
                        lpdm = null;
                    }
                }
                catch (Exception ex)
                {
                    lpdm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lpdm;
        }

        private List<PaymentApplyModel> getPaymentAppliesByORNumber(string ornumber)
        {
            List<PaymentApplyModel> lpam = new List<PaymentApplyModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getPaymentAppliesByORNumber";

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@ornumber", ornumber);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string schrge = string.Empty;
                            if (dr["surcharge"].ToString() != "0.00")
                                schrge = Convert.ToDouble(dr["surcharge"]).ToString("N2");

                            string scdisc = string.Empty;
                            if (dr["scdisc"].ToString() != "0.00")
                                scdisc = Convert.ToDouble(dr["scdisc"]).ToString("N2");

                            string duedate = string.Empty;
                            if (dr["duedate"].ToString() != "")
                                duedate = Convert.ToDateTime(dr["duedate"]).ToString("MM-dd-yyyy");
                            
                            string billdate = string.Empty;
                            if (dr["billingdate"] != null)
                                if (dr["billingdate"].ToString() != "TOTAL AR")
                                    billdate = Convert.ToDateTime(dr["billingdate"]).ToString("MM-d-yyyy");
                                else
                                    billdate = dr["billingdate"].ToString();

                            lpam.Add(new PaymentApplyModel
                            {
                                BillingDate = billdate,
                                Remarks = dr["remarks"].ToString().Trim(),
                                DueDate = duedate,
                                Amount = Convert.ToDouble(dr["amount"]).ToString("N2"),
                                VAT = Convert.ToDouble(dr["vat"]).ToString("N2"),
                                Surcharge = schrge,
                                Total = Convert.ToDouble(dr["total"]).ToString("N2"),
                                SCDisc = scdisc
                            });

                        }
                    }
                    else
                    {
                        lpam = null;
                    }
                }
                catch (Exception ex)
                {
                    lpam = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lpam;
        }

        private CompareMasterPaymentModel getComparisonMasterPaymentConsumerIdByORNumber(string ornumber)
        {
            CompareMasterPaymentModel cmpm = new CompareMasterPaymentModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_getConsumerIdComparisonPayToMasterByORNumber";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ornumber", ornumber);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            cmpm.RealConsumerId = rdr["realconsumerid"].ToString();
                            cmpm.Name = rdr["name"].ToString();
                            cmpm.Address = rdr["address"].ToString();
                            cmpm.FakeConsumerId = rdr["fakeconsumerid"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    cmpm = null;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return cmpm;
        }

        private string isUpdateSuccess(string ornumber, string realconsumerid, string userid)
        {
            string result = string.Empty;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_updateSystemErrorByORNumber";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ornumber", ornumber);
                cmd.Parameters.AddWithValue("@realconsumerid", realconsumerid);
                cmd.Parameters.AddWithValue("@userid", userid);

                try
                {
                    cmd.ExecuteNonQuery();
                    result = "SUCCESS";
                }
                catch (Exception ex)
                {
                    result = "An error occured: " + ex.Message;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }

            }

                return result;
        }
    }
}