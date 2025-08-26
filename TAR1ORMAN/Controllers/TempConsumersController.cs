using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.Configuration;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Reflection;

namespace TAR1ORMAN.Controllers
{
    public class TempConsumersController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        // GET: TempConsumers
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

        public JsonResult loadfordata()
        {
            return Json(new { data = GetAllTemporaryConsumers() }, JsonRequestBehavior.AllowGet);
        }


        //FUNCTIONS AND PROCEDURES
        public List<TempConsumerModel> GetAllTemporaryConsumers()
        {
            List<TempConsumerModel> lstscm = new List<TempConsumerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select cons.consumerid[accountno],rtrim(name)[name], rtrim(address)[address]," +
                    "rtrim(mtrserialno)[meterno], rtrim(poleid)[poleno], sum(trxbalance) trxbalance, " +
                    "sum(vatbalance) vatbalance, UPPER(rtrim(stat.description))[status], " +
                    "CASE WHEN cons.dateinstalled IS NOT NULL THEN CONVERT(varchar(10),cons.dateinstalled,101) " +
                    "ELSE '' END dateinstalled " +
                    "from arsconsumer cons inner join arstrxhdr hdr " +
                    "on cons.consumerid = hdr.consumerid " +
                    "inner join arsstatus stat " +
                    "on cons.statusid = stat.statusid " +
                    "where consumertypeid = 'T' " +
                    "group by cons.consumerid,name,address,mtrserialno,poleid,stat.description, cons.dateinstalled";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstscm.Add(new TempConsumerModel
                            {
                                AccountNo = dr["accountno"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                MeterNo = dr["meterno"].ToString(),
                                PoleNo = dr["poleno"].ToString(),
                                TrxBalance = Convert.ToDouble(dr["trxbalance"]),
                                VATBalance = Convert.ToDouble(dr["vatbalance"]),
                                Status = dr["status"].ToString(),
                                DateInstalled = dr["dateinstalled"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstscm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstscm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstscm;
        }
    }
}