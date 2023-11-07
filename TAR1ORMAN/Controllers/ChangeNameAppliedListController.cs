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

        private List<ChangeNameAppliedModel> getAllAppliedChangeName()
        {
            List<ChangeNameAppliedModel> lstcnam = new List<ChangeNameAppliedModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,appdate,old_name,accountno,co.address,isnull(old_memberid,'')[old_memberid],old_memberdate," +
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
                            lstcnam.Add(new ChangeNameAppliedModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                AppDate = Convert.ToDateTime(dr["appdate"]).ToString("MM/dd/yyyy"),
                                AccountNo = dr["accountno"].ToString(),
                                OldName = dr["old_name"].ToString(),
                                Address = dr["address"].ToString(),
                                OldMemberId = dr["old_memberid"].ToString(),
                                OldMemberDate = dr["old_memberdate"].ToString(),
                                NewName = dr["nw_name"].ToString(),
                                Status = dr["appstatus"].ToString()
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
    }
}