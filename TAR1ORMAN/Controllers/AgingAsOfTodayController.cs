using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Configuration;

namespace TAR1ORMAN.Controllers
{
    public class AgingAsOfTodayController : Controller
    {
        [Authorize(Roles = "AUDIT,FINHEAD,IT,SYSADMIN,AREAMNGR")]
        // GET: AgingAsOfToday
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllConsumerTypes()
        {
            return Json(new { data = getAllConsumerTypes() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllTowns() {
            return Json(new { data = getAllTowns() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentDate()
        {
            return Json(getCurrentDate(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckIfHasData(string townid, string typeid)
        {
            bool result = false;

            result = HasData(townid,typeid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData(string townidtypeid)
        {
            string[] strparams = townidtypeid.Split('~');

            List<AgingModel> lstam = new List<AgingModel>();
            lstam = loadData(strparams[0], strparams[1]);

            var jsonresult = Json(new { data = lstam }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        //PRIVATE PROCEDURES
        private List<ConsumerTypeModel> getAllConsumerTypes()
        {
            List<ConsumerTypeModel> lstctm = new List<ConsumerTypeModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select RTRIM(consumertypeid)[consumertypeid],UPPER(RTRIM(description))[type] from arstype;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstctm.Add(new ConsumerTypeModel
                            {
                                Id = dr["consumertypeid"].ToString(),
                                ConsumerType = dr["type"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstctm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstctm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstctm;
        }

        private List<TownModel> getAllTowns()
        {
            List<TownModel> lsttm = new List<TownModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select RTRIM(townid)[townid],UPPER(RTRIM(description))[town] from arstown;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsttm.Add(new TownModel
                            {
                                Id = dr["townid"].ToString(),
                                Town = dr["town"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lsttm = null;
                    }
                }
                catch (Exception ex)
                {
                    lsttm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lsttm;
        }

        private string getCurrentDate()
        {
            string retval = string.Empty;

            string constr = ConfigurationManager.ConnectionStrings["getconnstr"].ToString();

            SqlConnection con = new SqlConnection(constr);
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select convert(varchar,getdate(),110)[currdate];";

                SqlDataReader dr;

                try
                {
                    dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            retval = dr["currdate"].ToString();
                        }
                    }
                }
                catch(Exception ex)
                {
                    retval = "DATE_UNKNOWN";
                }
                finally
                {
                    con.Close();
                }
            }

            return retval;
        }

        private List<AgingModel> loadData(string townid, string typeid)
        {
            List<AgingModel> lstam = new List<AgingModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_agingasoftoday";

                da.SelectCommand.Parameters.AddWithValue("@typeid", typeid);
                da.SelectCommand.Parameters.AddWithValue("@townid", townid);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstam.Add(new AgingModel
                            {
                                TownId = dr["townid"].ToString(),
                                AccountNo = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                Status = dr["status"].ToString(),
                                ConsumerType = dr["type"].ToString(),
                                Days30 = Convert.ToDouble(dr["0 - 30 days"]),
                                Days60 = Convert.ToDouble(dr["31 - 60 days"]),
                                Days90 = Convert.ToDouble(dr["61 - 90 days past due"]),
                                Days120 = Convert.ToDouble(dr["91 - 120 days past due"]),
                                Above120 = Convert.ToDouble(dr["121 and above"])
                            });
                        }
                    }
                    else
                    {
                        lstam = null;
                    }
                }
                catch (Exception ex)
                {
                    lstam = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstam;
        }

        private bool HasData(string townid, string typeid)
        {
            bool result = false;

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_agingasoftoday";

                da.SelectCommand.Parameters.AddWithValue("@typeid", typeid);
                da.SelectCommand.Parameters.AddWithValue("@townid", townid);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return result;
        }
    }
}