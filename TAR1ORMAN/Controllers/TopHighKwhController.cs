using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class TopHighKwhController : Controller
    {
        private static GridView gv = new GridView();
        // GET: TopHighKwh
        [Authorize(Roles = "AREAMNGR,AUDIT,FINHEAD,IT,SYSADMIN")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult loadfordata(string billperiodtop)
        {
            string[] _bperiodtop = billperiodtop.Split('_');
            string bp = _bperiodtop[0];
            string top = _bperiodtop[1];

            //itads = new TopActiveDisconService();
            List<ConsumerHighKwhModel> data = this.GetResidentialConsumersHighKwh(bp, top);
            if (gv.DataSource != null)
                gv.DataSource = null;
            gv.DataSource = data;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBillPeriod()
        {
            return Json(GetBillPeriods(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();
            Response.AddHeader("content-disposition", "attachment; filename=TopResHighKwh_" + datenow + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }


        //functions
        private List<ConsumerHighKwhModel> GetResidentialConsumersHighKwh(string billperiod, string top)
        {
            //hdr.consumerid,cons.name,cons.address,cons.poleid,cons.mtrserialno,hdr.energyused[consumedKwh]
            List<ConsumerHighKwhModel> lsttopKwh = new List<ConsumerHighKwhModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_Top300ResHighKwh";

                da.SelectCommand.Parameters.AddWithValue("@billperiod", billperiod);
                da.SelectCommand.Parameters.AddWithValue("@top", Convert.ToInt32(top));

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsttopKwh.Add(new ConsumerHighKwhModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                PoleId = dr["poleid"].ToString(),
                                MeterNo = dr["mtrserialno"].ToString(),
                                KwH = Convert.ToInt32(dr["consumedKwh"]),
                                Amount = Convert.ToDouble(dr["amount"])
                            });
                        }
                    }
                    else
                    {
                        lsttopKwh = null;
                    }
                }
                catch (Exception ex)
                {
                    lsttopKwh = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lsttopKwh;
        }

        private List<BillPeriodModel> GetBillPeriods()
        {
            List<BillPeriodModel> lstBP = new List<BillPeriodModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select billperiod [Id],description [BillPeriod] " +
                                               "from arsbillperiod " +
                                               "order by seq; ";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstBP.Add(new BillPeriodModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                BillPeriod = dr["BillPeriod"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstBP = null;
                    }
                }
                catch (Exception ex)
                {
                    lstBP = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstBP;
        }
    }
}