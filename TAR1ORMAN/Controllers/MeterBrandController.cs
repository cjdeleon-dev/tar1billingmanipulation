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
    public class MeterBrandController : Controller
    {
        [Authorize(Roles = "IT,SYSADMIN,TEMPO,AUDIT,AREAMNGR,BILLING,FINHEAD,MDTO,MSERVE")]
        // GET: MeterBrand
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllMeterBrands()
        {
            var jsonResult = Json(new { data = getAllMeterBrands() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetAllMeterTypesByBrandId(int brandid)
        {
            var jsonResult = Json(new { data = getAllMeterTypesByBrandId(brandid) }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        //procedures and functions
        private List<MeterBrandModel> getAllMeterBrands()
        {
            List<MeterBrandModel> lmbm = new List<MeterBrandModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,brand,isactive from b_meterbrands;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmbm.Add(new MeterBrandModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                MeterBrand = dr["brand"].ToString(),
                                IsActive = Convert.ToBoolean(dr["isactive"])
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lmbm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lmbm;
        }

        private List<MeterTypeModel> getAllMeterTypesByBrandId(int id)
        {
            List<MeterTypeModel> lmtm = new List<MeterTypeModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,[type],isactive from b_metertypes where brandid=@brandid;";
                da.SelectCommand.Parameters.AddWithValue("@brandid", id);

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmtm.Add(new MeterTypeModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                MeterType = dr["type"].ToString(),
                                IsActive = Convert.ToBoolean(dr["isactive"])
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lmtm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lmtm;
        }
    }
}