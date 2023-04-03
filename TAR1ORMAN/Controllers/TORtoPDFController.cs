using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.AuditTrailService;

namespace TAR1ORMAN.Controllers
{
    public class TORtoPDFController : Controller
    {
        private string[] selDateRange;
        private DataTable dtHeaderData;
        private DataTable dtDetailTransData;
        private DataTable dtDetailTransNoVatData;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,SYSADMIN,MSERVE,TRAINEE")]

        // GET: TORtoPDF
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TORReportView(string rptParams)
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string datenow = DateTime.Now.ToShortDateString();
            string errmsg = string.Empty;

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            string p = Path.Combine(Server.MapPath("/Reports"), "rptSLIP.rdlc");
            lr.ReportPath = p;

            //initialize parameters
            selDateRange = rptParams.Split('_');

            string dateFr = selDateRange[0];
            string dateTo = selDateRange[1];
            string office = selDateRange[2];

            //data
            dtHeaderData = getHeaderData(dateFr, dateTo,office);
            dtDetailTransData = getDetailTransData(dateFr, dateTo,office);
            dtDetailTransNoVatData = getDetailTransNoVatData(dateFr, dateTo,office);

            //Subreporting
            lr.SubreportProcessing += new SubreportProcessingEventHandler(TORSubreportProcessingEventHandler);

            //ReportDataSource for Header
            ReportDataSource soahdr = new ReportDataSource("dsRptTORHeader", dtHeaderData);

            lr.DataSources.Add(soahdr);//Header

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 1; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Generate TOR data report from " + dateFr + " to " + dateTo + " with office of: " + office + ".";
            atm.TableAffected = "arspaytrxhdr, arspaytrxdtl, arspaytrxapply";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }


        public void TORSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {

            var ornum= e.Parameters["ORNumber"].Values.FirstOrDefault();
            
            e.DataSources.Add(new ReportDataSource("dsRptTORDetailTrans", dtDetailTransData.Select("ORNumber='" + ornum + "'").CopyToDataTable()));
            e.DataSources.Add(new ReportDataSource("dsTORDetailTransNoVat", dtDetailTransNoVatData.Select("ORNumber='" + ornum + "'").CopyToDataTable()));
        }

        public JsonResult GetAllOffices()
        {
            return Json(getAllTar1Offices(), JsonRequestBehavior.AllowGet);

        }


        private DataTable getHeaderData(string fr, string to, string officeid)
        {
            DataTable dtResult = new DataTable();

            DateTime datefr = Convert.ToDateTime(fr);
            DateTime dateto = Convert.ToDateTime(to);

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_1GetTORHeaderByDateRange";

                da.SelectCommand.Parameters.AddWithValue("@DateFrom", datefr);
                da.SelectCommand.Parameters.AddWithValue("@DateTo", dateto);
                da.SelectCommand.Parameters.AddWithValue("@officeid", Convert.ToInt32(officeid));
                try
                {
                    da.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    dtResult = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return dtResult;
        }

        private DataTable getDetailTransData(string fr, string to, string officeid)
        {
            DataTable dtResult = new DataTable();

            DateTime datefr = Convert.ToDateTime(fr);
            DateTime dateto = Convert.ToDateTime(to);

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_2GetTORAllTrxTotalAmountByDateRange";

                da.SelectCommand.Parameters.AddWithValue("@DateFrom", datefr);
                da.SelectCommand.Parameters.AddWithValue("@DateTo", dateto);
                da.SelectCommand.Parameters.AddWithValue("@officeid", Convert.ToInt32(officeid));

                try
                {
                    da.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    dtResult = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return dtResult;
        }

        private DataTable getDetailTransNoVatData(string fr, string to, string officeid)
        {
            DataTable dtResult = new DataTable();

            DateTime datefr = Convert.ToDateTime(fr);
            DateTime dateto = Convert.ToDateTime(to);

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_3GetTORTrxAmountByDateRange";

                da.SelectCommand.Parameters.AddWithValue("@DateFrom", datefr);
                da.SelectCommand.Parameters.AddWithValue("@DateTo", dateto);
                da.SelectCommand.Parameters.AddWithValue("@officeid", Convert.ToInt32(officeid));

                try
                {
                    da.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    dtResult = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return dtResult;
        }

        private List<OfficeModel> getAllTar1Offices()
        {
            List<OfficeModel> lstom = new List<OfficeModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id [Id], UPPER(office) [Office] from tbloffices;";

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstom.Add(new OfficeModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Office = dr["Office"].ToString()
                            });
                        }

                    }
                    else
                        lstom = null;
                }
                catch (Exception ex)
                {
                    lstom = null;
                }
                finally
                {
                    con.Close();
                }
            }

            return lstom;
        }
    }
}