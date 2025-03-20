using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class PoleRentalPaymentListController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,SYSADMIN")]
        // GET: PoleRentalList
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPoleRentPaymentByDateRange(string dateFr, string dateTo)
        {
            var jsonResult = Json(new { data = getPoleRentPaymentByDateRange(dateFr, dateTo) }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        private List<PoleRentalPaymentModel> getPoleRentPaymentByDateRange(string dtfrom, string dtto)
        {
            List<PoleRentalPaymentModel> lstprpm = new List<PoleRentalPaymentModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select hdr.ornumber,hdr.payee,hdr.address,trxdate,sum(dtl.trxamount)[amount],sum(dtl.vat) [vat],sum(dtl.amount) [totalamount] " +
                                               "from arspaytrxdtl dtl inner join arspaytrxhdr hdr on dtl.ornumber=hdr.ornumber " +
                                               "where dtl.trxid='PR' and hdr.trxdate between @datefr and @dateto " +
                                               "group by hdr.ornumber,hdr.payee,hdr.address,hdr.trxdate " +
                                               "order by trxdate";

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@datefr", dtfrom);
                da.SelectCommand.Parameters.AddWithValue("@dateto", dtto);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstprpm.Add(new PoleRentalPaymentModel
                            {
                                ORNumber = dr["ornumber"].ToString(),
                                Payee = dr["payee"].ToString(),
                                Address = dr["address"].ToString(),
                                TrxDate = Convert.ToDateTime(dr["trxdate"]).ToString("MM-dd-yyyy"),
                                Amount = Convert.ToDouble(dr["amount"]),
                                VAT = Convert.ToDouble(dr["vat"]),
                                TotalAmount = Convert.ToDouble(dr["totalamount"])
                            });

                        }
                    }
                    else
                    {
                        lstprpm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstprpm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstprpm;
        }
    }
}