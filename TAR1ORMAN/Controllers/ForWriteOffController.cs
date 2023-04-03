using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataService.ForWriteOffService;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TAR1ORDATA.DataModel;
using System.Collections.Generic;

namespace TAR1ORMAN.Controllers
{
    public class ForWriteOffController : Controller
    {

        IForWriteOffService ifwos;
        [Authorize(Roles = "AUDIT,FINHEAD,IT,SYSADMIN")]
        // GET: ForWriteOff
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult loadfordata(int yr,string area)
        {
            ifwos = new ForWriteOffService();
            var data = ifwos.GetForWriteOffList(yr,area);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportToExcel(int yr, string area)
        {
            ifwos = new ForWriteOffService();
            var gv = new GridView();
            gv.DataSource = ifwos.GetForWriteOffList(yr,area);
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();
            Response.AddHeader("content-disposition", "attachment; filename=" + area + "_" + yr + "_FORWRITEOFF.xls");
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

        public JsonResult GetAllYearOf()
        {
            ifwos = new ForWriteOffService();
            return Json(ifwos.GetAllYearOf(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAreaOf()
        {
            return Json(getAllAreaOf(), JsonRequestBehavior.AllowGet);
        }

        private List<AreaModel> getAllAreaOf()
        {
            List<AreaModel> lstarm = new List<AreaModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select * from (VALUES(1, 'GERONA'),(2, 'VICTORIA'),(3, 'PURA'),(4, 'PANIQUI'),(5, 'RAMOS')," +
                     "(6, 'MONCADA'),(7, 'SAN MANUEL'),(8, 'CUYAPO'),(9, 'ANAO'),(10, 'NAMPICUAN')," +
					 "(11, 'STA. IGNACIA'),(12, 'SAN JOSE'),(13, 'CAMILING'),(14, 'MAYANTOC'),(15, 'SAN CLEMENTE')) q(id, areadesc); ";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstarm.Add(new AreaModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Area = dr["areadesc"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstarm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstarm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstarm;
        }
    }
}