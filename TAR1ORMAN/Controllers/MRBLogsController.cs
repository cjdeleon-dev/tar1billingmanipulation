using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Reflection;
using System.Web.UI.WebControls;

namespace TAR1ORMAN.Controllers
{
    public class MRBLogsController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,SYSADMIN,BILLING")]
        // GET: MRBLogs
        public ActionResult MRBLog()
        {
            return View();
        }

        public JsonResult loaddata(string seldate)
        {
            var jsonResult = Json(new { data = getDataByDate(seldate) }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private List<MRBLogModel> getDataByDate(string selectedDate)
        {
            List<MRBLogModel> lst = new List<MRBLogModel>();
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select UPPER(RTRIM(unt.name))[name],hdr.routeid,CONVERT(VARCHAR(8), MIN(trxdate), 108)[start],CONVERT(VARCHAR(8), MAX(trxdate), 108)[end], " +
                                               "CAST(CAST(DATEDIFF(MINUTE, MIN(trxdate), MAX(trxdate)) AS NUMERIC(18, 2)) / 60.0 AS NUMERIC(18, 2))[totalhours] " +
                                               "from arsbatchdtl dtl inner join arsbatchhdr hdr " +
                                               "on dtl.batchid = hdr.batchid " +
                                               "inner join arsunit unt " +
                                               "on hdr.unitid = unt.unitid " +
                                               "where CAST(trxdate AS DATE) = @selDate " +
                                               "and dateposted is not null " +
                                               "group by hdr.routeid,unt.name";

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@selDate", selectedDate);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            lst.Add(new MRBLogModel
                            {
                                Name = dr["name"].ToString(),
                                RouteId = dr["routeid"].ToString(),
                                Start = dr["start"].ToString(),
                                End = dr["end"].ToString(),
                                Total = Convert.ToDouble(dr["totalhours"])
                            });
                        }
                    }
                    else
                    {
                        lst = null;
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
    }
}