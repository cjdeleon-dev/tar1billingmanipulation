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

namespace TAR1ORMAN.Controllers
{
    public class SOAController : Controller
    {

        private string[] routeBPeriod;
        private DataTable dtHeaderData;
        private DataTable dtDetailsData;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,SYSADMIN,MSERVE,TRAINEE")]

        // GET: SOA
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SOAReportView(string rptParams)
        {
            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptSOA.rdlc");
            lr.ReportPath = p;

            //initialize parameters
             routeBPeriod = rptParams.Split('_');

            dtHeaderData = getHeaderData(routeBPeriod[0], routeBPeriod[1]);
            dtDetailsData = getDetailsData(routeBPeriod[0], routeBPeriod[1]);

            lr.SubreportProcessing += new SubreportProcessingEventHandler(SOASubreportProcessingEventHandler); 

             //ReportDataSource for Header
             ReportDataSource soahdr = new ReportDataSource("dsRptSOAHeader", dtHeaderData);

            lr.DataSources.Add(soahdr);//Header
 
            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        void SOASubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {

            var acctno = e.Parameters[0].Values[0].ToString();

            e.DataSources.Add(new ReportDataSource("dsReportDetails", dtDetailsData.Select("AccountNumber='" + acctno + "' and CategoryId<>5").CopyToDataTable()));
            e.DataSources.Add(new ReportDataSource("dsSOAReading", dtHeaderData.Select("AccountNumber='" + acctno + "'").CopyToDataTable()));
            e.DataSources.Add(new ReportDataSource("dsRptSOAVATDetail", dtDetailsData.Select("AccountNumber='" + acctno + "' and CategoryId=5").CopyToDataTable()));
            e.DataSources.Add(new ReportDataSource("dsRptSOAVATHeader", dtHeaderData.Select("AccountNumber='" + acctno + "'").CopyToDataTable()));
        }

        private DataTable getHeaderData(string routeid, string billperiod)
        {
            DataTable dtResult = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();
                
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getSOAHeaderByRouteIdBillPeriod";

                da.SelectCommand.Parameters.AddWithValue("@routeid", routeid);
                da.SelectCommand.Parameters.AddWithValue("@billperiod", billperiod);
                try
                {
                    da.Fill(dtResult);
                }
                catch(Exception ex)
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

        private DataTable getDetailsData(string routeid, string billperiod)
        {
            DataTable dtResult = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = con;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getSOAUnbundledByRouteIdBillPeriod";

                da.SelectCommand.Parameters.AddWithValue("@routeid", routeid);
                da.SelectCommand.Parameters.AddWithValue("@billperiod", billperiod);

                try
                {
                    da.Fill(dtResult);
                }
                catch
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

        [HttpGet]
        public JsonResult getAllRoutes()
        {
            List<RouteModel> lstrm = new List<RouteModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select routeid [id], routeid [route] from arsroute;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstrm.Add(new RouteModel
                            {
                                Id = dr["id"].ToString(),
                                Route = dr["route"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstrm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstrm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstrm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getAllBillPeriods()
        {
            List<BillPeriodModel> lstbpm = new List<BillPeriodModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select rtrim(billperiod)[Id], rtrim(billperiod)[Description] from arsbillperiod where billperiod>=201401;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstbpm.Add(new BillPeriodModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                BillPeriod = dr["Description"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstbpm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstbpm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstbpm, JsonRequestBehavior.AllowGet);

        }
        //


    }
}