using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class FourPsApplicationController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: FourPsApplication
        public ActionResult FourPsVerify()
        {
            return View();
        }

        public JsonResult VerifyFourPsDetail(string name)
        {
            List<FourPsModel> lstfpm = new List<FourPsModel>();

            string[] arrName = name.Split('~');
            if (arrName.Length == 3)
            {
                lstfpm = verifyByName(arrName[0], arrName[1], arrName[2]);
            }

            return Json(new { data = lstfpm }, JsonRequestBehavior.AllowGet);
        }







        //FUNCTIONS AND PROCEDURES
        private List<FourPsModel> verifyByName(string lastname, string firstname, string midname)
        {
            List<FourPsModel> lst = new List<FourPsModel>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select psgc_bgy,hh_id,entry_id, last_name + ', ' + first_name + ' ' + middle_name[name]," +
                                      "brgy_name + ', ' + city_name + ', ' + prov_name[address],birthday,sex " +
                                      "from tbl_mstrfourps " +
                                      "where COALESCE(last_name, '') like COALESCE(@lname+'%',last_name,'') " +
                                      "and COALESCE(first_name,'') like COALESCE(@fname+'%',first_name,'') " +
                                      "and COALESCE(middle_name,'') like COALESCE(@mname+'%',middle_name,'');";

                    cmd.Parameters.Clear();
                    if(lastname==string.Empty)
                        cmd.Parameters.AddWithValue("@lname", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@lname", lastname);

                    if (firstname == string.Empty)
                        cmd.Parameters.AddWithValue("@fname", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fname", firstname);

                    if (midname == string.Empty)
                        cmd.Parameters.AddWithValue("@mname", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@mname", midname);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new FourPsModel {
                                PSGC_Bgy = rdr["psgc_bgy"].ToString(),
                                HH_Id = rdr["hh_id"].ToString(),
                                EntryId = rdr["entry_id"].ToString(),
                                Name = rdr["name"].ToString(),
                                Address = rdr["address"].ToString(),
                                Birthday = Convert.ToDateTime(rdr["birthday"]).ToShortDateString(),
                                Gender = rdr["sex"].ToString()
                            });
                        }
                    }


                }
                catch (Exception)
                {
                    lst = null;
                }
                finally
                {
                    cmd.Dispose();
                }

            }

            return lst;
        }
    }
}