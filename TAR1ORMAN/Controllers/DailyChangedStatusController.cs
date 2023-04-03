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

namespace TAR1ORMAN.Controllers
{
    public class DailyChangedStatusController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MSERVE,SYSADMIN,TELLER")]

        // GET: DailyChangedStatus
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewChangedStatusReport(string rptParams)
        {
            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptChangedStatus.rdlc");
            lr.ReportPath = p;

            //initialize parameters
            string userid = User.Identity.Name;

            string[] parameters = rptParams.Split('_');

            string status = string.Empty;

            if (parameters[0].ToString() == "1")
                status = "A";
            if (parameters[0].ToString() == "2")
                status = "D";


            DataTable dtChangedStatus = new DataTable();
            dtChangedStatus = getChangedStatusList(userid,status, parameters[1]);

            //ReportDataSource
            ReportDataSource cstatlist = new ReportDataSource("dsChangedStatus", dtChangedStatus);

            lr.DataSources.Add(cstatlist);

            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        private DataTable getChangedStatusList(string userid, string statusid, string datechange)
        {

            DataTable dtResult = new DataTable();

            DateTime datefr = Convert.ToDateTime((datechange + " 00:00:00"));
            DateTime dateto = Convert.ToDateTime((datechange + " 23:59:59"));

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getAllChangedStatByUserIdAndDate";

                da.SelectCommand.Parameters.AddWithValue("@DateFrom", datefr);
                da.SelectCommand.Parameters.AddWithValue("@DateTo", dateto);
                da.SelectCommand.Parameters.AddWithValue("@Userid", userid);
                da.SelectCommand.Parameters.AddWithValue("@Statusid", statusid);

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
    }
}