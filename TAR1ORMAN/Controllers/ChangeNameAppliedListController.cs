using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TAR1ORMAN.Controllers
{
    public class ChangeNameAppliedListController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: ChangeNameAppliedList
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult loaddata()
        {
            var jsonResult = Json(new { data = getAllAppliedChangeName()}, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult UpdateCNMember(AppCNMemModel acnm)
        {
            string userid = User.Identity.Name;
            acnm.UpdatedBy = userid;

            if (isSuccessUpdateCNMember(acnm))
                return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Fail To Update." }, JsonRequestBehavior.AllowGet);
        }

        private List<ChangeNameAppliedModel> getAllAppliedChangeName()
        {
            List<ChangeNameAppliedModel> lstcnam = new List<ChangeNameAppliedModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,appdate,old_name,accountno,co.address,isnull(old_memberid,'')[old_memberid],old_memberdate[old_memberdate],remarks," +
                                               "nw_name,appstatus " +
                                               "from tbl_changename cn inner join arsconsumer co " +
                                               "on cn.accountno=co.consumerid " +
                                               "where isnull(appstatus,'')<>'DONE';";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string oldmemdate = dr["old_memberdate"].ToString();

                            lstcnam.Add(new ChangeNameAppliedModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                AppDate = Convert.ToDateTime(dr["appdate"]).ToString("MM/dd/yyyy"),
                                AccountNo = dr["accountno"].ToString(),
                                OldName = dr["old_name"].ToString(),
                                Address = dr["address"].ToString(),
                                OldMemberId = dr["old_memberid"].ToString(),
                                OldMemberDate = oldmemdate==""?null:Convert.ToDateTime(dr["old_memberdate"]).ToString("yyyy-MM-dd"),
                                NewName = dr["nw_name"].ToString(),
                                Status = dr["appstatus"].ToString(),
                                Remark = dr["remarks"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstcnam = null;
                    }
                }
                catch (Exception ex)
                {
                    lstcnam = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstcnam;
        }

        private bool isSuccessUpdateCNMember(AppCNMemModel acnm)
        {
            bool result = true;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update tbl_changename set nw_name=@nw_name,old_memberid=@memberid, old_memberdate=@memberdate, remarks=@remarks," +
                                  "lastupdated=getdate(),updatedby=@updatedby where id=@refid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nw_name", acnm.NewName);
                cmd.Parameters.AddWithValue("@memberid", acnm.OldMemberId);
                cmd.Parameters.AddWithValue("@memberdate", acnm.OldMemDate);
                cmd.Parameters.AddWithValue("@remarks", acnm.Remark);
                cmd.Parameters.AddWithValue("@updatedby", acnm.UpdatedBy);
                cmd.Parameters.AddWithValue("@refid", acnm.RefId);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
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
    }
}