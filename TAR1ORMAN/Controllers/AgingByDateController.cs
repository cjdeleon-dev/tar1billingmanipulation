using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class AgingByDateController : Controller
    {
        [Authorize(Roles = "AUDIT,FINHEAD,IT,SYSADMIN,AREAMNGR")]
        // GET: AgingByDate
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData(string sdate, bool isEB=true)
        {

            List<AgingAcctModel> lstam = new List<AgingAcctModel>();
            lstam = loadData(sdate,isEB);

            var jsonresult = Json(new { data = lstam }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        //FUNCTIONS AND PROCEDURES
        private List<AgingAcctModel> loadData(string sdate, bool isEB)
        {
            List<AgingAcctModel> lstam = new List<AgingAcctModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.CommandTimeout = 1800;
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_agingacctformat";

                da.SelectCommand.Parameters.AddWithValue("@dateend", sdate);
                da.SelectCommand.Parameters.AddWithValue("@isEB", isEB);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstam.Add(new AgingAcctModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                Status = dr["statusid"].ToString(),
                                ConsumerType = dr["consumertypeid"].ToString(),
                                Days30 = Convert.ToDouble(dr["0 - 30 days"]),
                                Days60 = Convert.ToDouble(dr["31 - 60 days"]),
                                Days90 = Convert.ToDouble(dr["61 - 90 days past due"]),
                                Days180 = Convert.ToDouble(dr["91 - 180 days  past due"]),
                                Days240 = Convert.ToDouble(dr["181 - 240 days  past due"]),
                                Days360 = Convert.ToDouble(dr["241 - 360 days  past due"]),
                                AboveDays365 = Convert.ToDouble(dr["over 1 (one year) past due"]),
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
    }
}