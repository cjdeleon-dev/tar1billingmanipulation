using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Net.NetworkInformation;
using System.Reflection;

namespace TAR1ORMAN.Controllers
{
    public class ChangeNameListForBODResController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: ChangeNameListForBODRes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult loaddata(string daterange)
        {
            string[] dates = daterange.Split('~');
            return Json(new { data = getAllChangeNameForReso(dates[0], dates[1]) }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult ExportToExcel()
        //{
        //    var gv = new GridView();
        //    gv.DataSource = getAllChangeNameForReso();
        //    gv.DataBind();
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    string datenow = DateTime.Now.ToShortDateString();
        //    Response.AddHeader("content-disposition", "attachment; filename="+Convert.ToDateTime(datenow).ToString("MM-dd-yyyy")+"_CNFORRESO.xls");
        //    Response.ContentType = "application/ms-excel";
        //    Response.Charset = "";
        //    StringWriter objStringWriter = new StringWriter();
        //    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
        //    gv.RenderControl(objHtmlTextWriter);
        //    Response.Output.Write(objStringWriter.ToString());
        //    Response.Flush();
        //    Response.End();

        //    return View("Index");
        //}




        //Procedures and functions
        private List<ChangeNameListForResoModel> getAllChangeNameForReso(string datefr, string dateto)
        {
            List<ChangeNameListForResoModel> lstcnlfr = new List<ChangeNameListForResoModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,appdate,old_name,co.address,isnull(old_memberid,'')[old_memberid],old_memberdate, " +
                                               "nw_name,isnull(nw_memberid, '')[nw_memberid],nw_memberdate,accountno,isnull(remarks, '')[remarks] " +
                                               "from tbl_changename cn inner join arsconsumer co " +
                                               "on cn.accountno = co.consumerid " +
                                               "where isnull(appstatus, '') = 'FOR BOARD RESOLUTION' " +
                                               "and ISNULL(exported,0)= 0 and dateexported IS NULL and dateapproved between @datefr and @dateto; ";

                da.SelectCommand.Parameters.AddWithValue("@datefr", datefr);
                da.SelectCommand.Parameters.AddWithValue("@dateto", dateto);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstcnlfr.Add(new ChangeNameListForResoModel
                            {
                               Id = Convert.ToInt32(dr["id"]),
                               ApplicationDate = dr["appdate"].ToString(),
                               NameOld = dr["old_name"].ToString(),
                               Address = dr["address"].ToString(),
                               ORNoOld = dr["old_memberid"].ToString(),
                               ORDateOld = dr["old_memberdate"].ToString(),
                               NameNew = dr["nw_name"].ToString(),
                               ORNoNew = dr["nw_memberid"].ToString(),
                               ORDateNew = dr["nw_memberdate"].ToString(),
                               AccountNo = dr["accountno"].ToString(),
                               Remarks = dr["remarks"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstcnlfr = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcnlfr = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcnlfr;
        }
    }
}