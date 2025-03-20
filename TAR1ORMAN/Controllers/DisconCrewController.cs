using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.IO.Pipes;

namespace TAR1ORMAN.Controllers
{
    public class DisconCrewController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        // GET: DisconCrew
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllDisconCrews()
        {
            var jsonResult = Json(new { data = getAllDisconCrews() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult InsertNewDisconCrew(DisconCrewModel dcm)
        {
            ProcessResultModel prm = new ProcessResultModel();
            prm = insertNewCrew(dcm);
            return Json(new { data = prm }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllOffices()
        {
            return Json(getAllOffices(), JsonRequestBehavior.AllowGet);
        }

        //PROCEDURES AND FUNCTIONS

        private List<DisconCrewModel> getAllDisconCrews()
        {
            List<DisconCrewModel> lst = new List<DisconCrewModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select a.id,crewname,b.office,isactive from tbl_disconcrews a inner join tbloffices b on a.officeid=b.id;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lst.Add(new DisconCrewModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Name = dr["crewname"].ToString(),
                                Office = dr["office"].ToString(),
                                IsActive = Convert.ToBoolean(dr["isactive"])
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lst = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lst;
        }

        private List<OfficeModel> getAllOffices()
        {
            List<OfficeModel> lstom = new List<OfficeModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,UPPER(RTRIM(office))office, RTRIM(displaytext)[displaytext] from tbloffices;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstom.Add(new OfficeModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Office = dr["office"].ToString(),
                                DisplayText = dr["displaytext"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstom = null;
                    }
                }
                catch (Exception ex)
                {
                    lstom = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstom;
        }

        private ProcessResultModel insertNewCrew(DisconCrewModel dcm)
        {
           ProcessResultModel prm = new ProcessResultModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into tbl_disconcrews values(@crewname,@officeid,1); " +
                                      "insert into tblAuditTrail values(2,'tbl_disconcrews','Insert New Disconnection Crew: ' + @crewname + '.', @userid, getdate());";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@crewname", dcm.Name.ToUpper());
                    cmd.Parameters.AddWithValue("@officeid", dcm.OfficeId);
                    cmd.Parameters.AddWithValue("@userid", User.Identity.Name);

                    cmd.ExecuteNonQuery();

                    prm.IsSucceed = true;
                    prm.ResultMessage = "Successfully Saved.";
                }
                catch (Exception ex) {
                    prm.IsSucceed = false;
                    prm.ResultMessage = ex.Message;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return prm;
        }
    }
}