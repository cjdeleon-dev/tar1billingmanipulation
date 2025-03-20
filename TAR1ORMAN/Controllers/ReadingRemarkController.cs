using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Reflection;

namespace TAR1ORMAN.Controllers
{
    public class ReadingRemarkController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,SYSADMIN,BILLING")]
        // GET: ReadingRemark
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllReadingRemarks()
        {
            var jsonResult = Json(new { data = getAllReadingRemarks() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        //functions and procedures

        private List<ReadingRemarkModel> getAllReadingRemarks()
        {
            List<ReadingRemarkModel> lrrm = new List<ReadingRemarkModel>();

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    try
                    {
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.Connection = con;
                        da.SelectCommand.CommandType = CommandType.Text;
                        da.SelectCommand.CommandText = "select dtl.consumerid,cons.name,cons.address,mtrserialno,dtl.errtxt,dtl.remarks " +
                                                       "from arsbatchhdr hdr inner join arsbatchdtl dtl " +
                                                       "on hdr.batchid = dtl.batchid " +
                                                       "left join arsconsumer cons " +
                                                       "on dtl.consumerid = cons.consumerid " + 
                                                       "where UPPER(remarks) <> 'DTD' " + 
                                                       "and RTRIM(UPPER(remarks))<> '0' " +
                                                       "and RTRIM(remarks)<> '';";


                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                lrrm.Add(new ReadingRemarkModel
                                {
                                    ConsumerId = dr["consumerid"].ToString(),
                                    Name = dr["name"].ToString(),
                                    Address = dr["address"].ToString(),
                                    MeterSerialNo = dr["mtrserialno"].ToString(),
                                    ErrText = dr["errtxt"].ToString(),
                                    Remark = dr["remarks"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lrrm = null;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return lrrm;
        }
    }
}