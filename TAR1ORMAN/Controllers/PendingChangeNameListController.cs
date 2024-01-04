using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class PendingChangeNameListController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: PendingChangeNameList
        public ActionResult Index()
        {
            if (User.IsInRole("SYSADMIN"))
            {
                ViewBag.Message = "ADMIN";
            }
            else
            {
                ViewBag.Message = "NONADMIN";
            }

            return View();
        }

        [HttpGet]
        public JsonResult loadfordata()
        {
            var jsonResult = Json(new { data = getAllPendingChangeNameList() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetDetailsOfChangeNameByRefId(int refid)
        {
            return Json(getAllPendingChangeNameList().Find(x=>x.RefID.Equals(refid)), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateNewORMember(MemberORModel mom)
        {
            string userid = User.Identity.Name;
            mom.UpdatedBy = userid;

            if (isSuccessUpdateMemberOR(mom))
                return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Fail To Update." }, JsonRequestBehavior.AllowGet);
        }


        private bool isSuccessUpdateMemberOR(MemberORModel m)
        {
            bool result = true;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update tbl_changename set nw_memberid=@memberid, nw_memberdate=@memberdate, appstatus='FOR GM''S APPROVAL'," +
                                  "lastupdated=getdate(),updatedby=@updatedby where id=@refid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@memberid", m.MemberOR);
                cmd.Parameters.AddWithValue("@memberdate", m.ORDate);
                cmd.Parameters.AddWithValue("@updatedby", m.UpdatedBy);
                cmd.Parameters.AddWithValue("@refid", m.RefId);

                try
                {
                    cmd.ExecuteNonQuery();
                }catch(Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                }
            }

            return result;

        }

        private List<PendingChangeNameModel> getAllPendingChangeNameList()
        {
            List<PendingChangeNameModel> lstpcnm = new List<PendingChangeNameModel>();

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
                        da.SelectCommand.CommandText = "select id,accountno,nw_name [name],appdate,nw_reason [reason],remarks " +
                                                       "from tbl_changename " +
                                                       "where appstatus='FOR PAYMENT';";

                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                lstpcnm.Add(new PendingChangeNameModel
                                {
                                    RefID = Convert.ToInt32(dr["id"]),
                                    AccountNumber = dr["accountno"].ToString(),
                                    Name = dr["name"].ToString(),
                                    AppDate = dr["appdate"].ToString(),
                                    Reason = dr["reason"].ToString(),
                                    Remarks = dr["remarks"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lstpcnm = null;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return lstpcnm;
        }
    }
}