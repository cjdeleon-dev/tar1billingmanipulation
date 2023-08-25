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
using TAR1ORDATA.DataService.AuditTrailService;
using TAR1ORDATA.DataService.DisconListService;

namespace TAR1ORMAN.Controllers
{
    public class DisconListController : Controller
    {
        IDisconListService idls; 
        
        private static GridView gv = new GridView();
        private static string selRouteId = string.Empty;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MSERVE,SYSADMIN,TELLER")]
        // GET: DisconList
        public ActionResult Index()
        {
            return View();
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
                da.SelectCommand.CommandText = "select routeid [id], description [route] from arsroute;";

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

        [HttpGet]
        public ActionResult loadfordata(string nomonthstatroute)
        {
            idls = new DisconListService();
            string[] _nomonthstatroute = nomonthstatroute.Split('_');
            int nomonth = Convert.ToInt32(_nomonthstatroute[0]);
            string stat = _nomonthstatroute[1]=="0"?"1": _nomonthstatroute[1];
            string route = _nomonthstatroute[2];

            selRouteId = route;

            //itads = new TopActiveDisconService();
            List<DisconListModel> data = idls.GetSubForDisconList(nomonth, stat, route);
            if (gv.DataSource != null)
                gv.DataSource = null;
            gv.DataSource = data;

            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            IAuditTrailService iats;

            iats = new AuditTrailService();
            
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();

            string errmsg = string.Empty;

            if (selRouteId == "0")
                selRouteId = "All";

            try
            {
                Response.AddHeader("content-disposition", "attachment; filename=ForDisconList_RouteId_" + selRouteId + "_" + datenow + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                //saving to audit trail
                AuditTrailModel atm = new AuditTrailModel();
                atm.Id = 0;
                atm.MadeById = User.Identity.Name;
                atm.ProcessTypeId = 1;
                atm.ProcessMade = "Generated and exported data for Subject for Disconnection List. Route Id: " + selRouteId;
                atm.TableAffected = "arsconsumer, arstrxhdr";
                atm.MadeDateTime = datenow;

                if (iats.LogtoAuditTrail(atm))
                    errmsg = "Success";
                else
                    errmsg = "Unable to log this process.";

                selRouteId = string.Empty;

                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                errmsg = "Unable to export.\n" + ex.Message;
            }
            
            return View("Index");
        }
    }
}