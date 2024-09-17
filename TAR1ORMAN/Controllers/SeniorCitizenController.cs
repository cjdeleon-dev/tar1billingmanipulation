using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.SeniorCitizenService;

namespace TAR1ORMAN.Controllers
{
    public class SeniorCitizenController : Controller
    {
        ISeniorCitizenService iscs;
        // GET: SeniorCitizen
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
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
        public ActionResult loadfordata()
        {
            iscs = new SeniorCitizenService();
            var data = iscs.GetAllSeniorCitizens();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountDetails(string accountNum)
        {
            MemberConsumerModel mcm = new MemberConsumerModel();
            mcm = getAccountDetailsByAcctNum(accountNum);
            return Json(mcm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSCIDOfAccountNo(string accountNum, string scidno)
        {
            string msg = string.Empty;

            if (updateSCIDofAccountNo(accountNum, scidno))
                msg = "Success";
            else
                msg = "Failed";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        private MemberConsumerModel getAccountDetailsByAcctNum(string acctnum)
        {
            MemberConsumerModel mcm = new MemberConsumerModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select consumerid,rtrim(name)[name],isnull(senioridno,'') senioridno " +
                                  "from arsconsumer " +
                                  "where consumerid = @consumerid";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            mcm.ConsumerId = acctnum;
                            mcm.Name = rdr["name"].ToString();
                            mcm.SCID = rdr["senioridno"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    mcm = null;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return mcm;
        }

        private bool updateSCIDofAccountNo(string accountNum, string scidno)
        {
            bool isSuccess = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "update arsconsumer " +
                                  "set senioridno=@scid " +
                                  "where consumerid = @consumerid";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@scid", scidno);
                cmd.Parameters.AddWithValue("@consumerid", accountNum);

                try
                {
                    cmd.ExecuteNonQuery();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return isSuccess;
        }
        
    }
}