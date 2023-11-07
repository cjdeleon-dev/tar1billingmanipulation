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
    public class ApprovalChangeNameListController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: ApprovalChangeNameList
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
            var jsonResult = Json(new { data = getAllApprovalChangeNameList() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public JsonResult ApprovedApplicant(int refid)
        {
            string usrid = User.Identity.Name.ToString();

            return Json(isSuccessApproval(refid, usrid), JsonRequestBehavior.AllowGet);
        }

        private List<ApprovalChangeNameModel> getAllApprovalChangeNameList()
        {
            List<ApprovalChangeNameModel> lstacnm = new List<ApprovalChangeNameModel>();

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
                        da.SelectCommand.CommandText = "select id,accountno,nw_name [name],appdate " +
                                                       "from tbl_changename " +
                                                       "where nw_memberid is not null and nw_memberdate is not null " +
                                                       "and isnull(isapproved,0)=0;";

                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                lstacnm.Add(new ApprovalChangeNameModel
                                {
                                    RefID = Convert.ToInt32(dr["id"]),
                                    AccountNumber = dr["accountno"].ToString(),
                                    Name = dr["name"].ToString(),
                                    AppDate = dr["appdate"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lstacnm = null;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return lstacnm;
        }


        private bool isSuccessApproval(int referenceid, string userid)
        {
            bool result = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                SqlTransaction trans = null;

                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = trans;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "declare @acctno varchar(10);" +
                                      "declare @newName varchar(MAX);" +
                                      "declare @newMemberId varchar(MAX);" +
                                      "declare @newMemberDate varchar(MAX);" +
                                      "set @acctno=(select accountno from tbl_changename where id=@id);" +
                                      "set @newName=(select rtrim(nw_name) from tbl_changename where id=@id);" +
                                      "set @newMemberId=(select rtrim(nw_memberid) from tbl_changename where id=@id);" +
                                      "set @newMemberDate=(select rtrim(nw_memberdate) from tbl_changename where id=@id);" +
                                      "update tbl_changename set isapproved=1,dateapproved=getdate(),appstatus='FOR BOARD RESOLUTION',lastupdated=getdate(),updatedby=@userid where id=@id; " +
                                      "update arsconsumer set name=@newName," +
                                      "                       memberid=@newMemberId," +
                                      "                       memberdate=@newMemberDate " +
                                      "where consumerid=@acctno; ";

                    cmd.Parameters.AddWithValue("@id", referenceid);
                    cmd.Parameters.AddWithValue("@userid", userid);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result = false;
                }
                finally
                {
                    trans.Dispose();
                    con.Close();
                }
            }

            return result;

        }
    }
}