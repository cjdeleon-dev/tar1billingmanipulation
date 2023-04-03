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

namespace TAR1ORMAN.Controllers
{
    public class ActiveWithZeroConsumptionController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        // GET: ActiveWithZeroConsumption
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult loadfordata()
        {
            var data = GetAllActiveConsumerWithZeroKWH();
            
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ExportToExcel()
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            var gv = new GridView();
            gv.DataSource = GetAllActiveConsumerWithZeroKWH();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();

            string errmsg = string.Empty;

            try
            {
                Response.AddHeader("content-disposition", "attachment; filename=ActiveZeroConsumption_" + "AsOf_" + datenow + ".xls");
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
                atm.ProcessTypeId = 1; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
                atm.ProcessMade = "Generated and exported data for Active Consumers with Zero Consumption.";
                atm.TableAffected = "arsconsumer, arstrxhdr";
                atm.MadeDateTime = datenow;

                if (iats.LogtoAuditTrail(atm))
                    errmsg = "Success";
                else
                    errmsg = "Unable to log this process.";

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



        private List<ConsumerZeroKWHModel> GetAllActiveConsumerWithZeroKWH()
        {
            List<ConsumerZeroKWHModel> lstczkm = new List<ConsumerZeroKWHModel>();
            DataTable dtresult = new DataTable();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                if (conn.State == ConnectionState.Broken)
                    conn.Close();

                conn.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandTimeout = 300;
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_getAllActiveConsumersWithZeroConsumption";

                try
                {
                    da.Fill(dtresult);
                    if (dtresult.Rows.Count > 0)
                    {
                        foreach(DataRow dr in dtresult.Rows)
                        {
                            lstczkm.Add(new ConsumerZeroKWHModel
                            {
                                AccountNo = dr["AccountNo"].ToString(),
                                AccountName = dr["AccountName"].ToString(),
                                Address = dr["Address"].ToString(),
                                MeterNo = dr["MeterNo"].ToString(),
                                PoleId = dr["PoleId"].ToString(),
                                Type = dr["Type"].ToString(),
                                ReadKWH = Convert.ToInt32(dr["ReadKWH"])
                            });
                        }
                        

                    }
                }
                catch (Exception)
                {
                    lstczkm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                    conn.Close();
                }

            }

                return lstczkm;
        }
    }
}