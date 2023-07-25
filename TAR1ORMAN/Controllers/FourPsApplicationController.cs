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


        public JsonResult CheckAccounts(string searchparams)
        {
            List<ConsumerModel> lstcons = new List<ConsumerModel>();
            string[] arrparams = searchparams.Split('~');
            if (arrparams.Length == 3)
            {
                lstcons = searchAccount(arrparams[0], arrparams[1], arrparams[2]);
            }

            return Json(new { data = lstcons }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFourPsDetailByEntryId(string entryId)
        {
            return Json(new { data = getFourPsDetailByEntryId(entryId) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertQualifiedLifeliner(FourPsModel fpm)
        {
            bool result = false;
            if (saveEntryQLifeLiner(fpm))
                result = true;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult PreviewQualifiedLifelinerReport()
        //{

        //}


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

        private List<ConsumerModel> searchAccount(string acctno, string name, string address)
        {
            List<ConsumerModel> lst = new List<ConsumerModel>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select consumerid,name,address " +
                                      "from arsconsumer " +
                                      "where COALESCE(consumerid, '') like COALESCE('%'+@acctno+'%',consumerid,'') " +
                                      "and COALESCE(name,'') like COALESCE('%'+@name+'%',name,'') " +
                                      "and COALESCE(address,'') like COALESCE('%'+@address+'%',address,'') and statusid='A';";

                    cmd.Parameters.Clear();
                    if (acctno == string.Empty)
                        cmd.Parameters.AddWithValue("@acctno", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@acctno", acctno);

                    if (name == string.Empty)
                        cmd.Parameters.AddWithValue("@name", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@name", name);

                    if (address == string.Empty)
                        cmd.Parameters.AddWithValue("@address", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@address", address);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new ConsumerModel
                            {
                                AccountNo = rdr["consumerid"].ToString(),
                                AccountName = rdr["name"].ToString(),
                                Address = rdr["address"].ToString()
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

        private FourPsModel getFourPsDetailByEntryId(string entryid)
        {
            FourPsModel fpm = new FourPsModel();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select prov_name,city_name,brgy_name,psgc_bgy,hh_id,entry_id," +
                                      "last_name,first_name,middle_name,ext_name,birthday,sex " +
                                      "from tbl_mstrfourps " +
                                      "where entry_id = @entryid";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@entryid", entryid);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            fpm.Provname = rdr["prov_name"].ToString();
                            fpm.Cityname = rdr["city_name"].ToString();
                            fpm.Brgyname = rdr["brgy_name"].ToString();
                            fpm.PSGC_Bgy = rdr["psgc_bgy"].ToString();
                            fpm.HH_Id = rdr["hh_id"].ToString();
                            fpm.EntryId = rdr["entry_id"].ToString();
                            fpm.Surname = rdr["last_name"].ToString();
                            fpm.Givenname = rdr["first_name"].ToString();
                            fpm.Middlename = rdr["middle_name"].ToString();
                            fpm.Extensionname = rdr["ext_name"].ToString();
                            fpm.Birthday = Convert.ToDateTime(rdr["birthday"]).ToString("yyyy-MM-dd");
                            fpm.Gender = rdr["sex"].ToString();
                        }
                    }


                }
                catch (Exception)
                {
                    fpm = null;
                }
                finally
                {
                    cmd.Dispose();
                }

            }

            return fpm;

        }

        private bool isExistAccountNo(string accountno)
        {
            bool result = false;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select count(*) numrows " +
                                      "from tbl_qualifiedFPQME " +
                                      "where consumerid = @consumerid";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@consumerid", accountno);

                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            if (Convert.ToInt32(rdr["numrows"]) > 0)
                                result = true;
                        }
                    }


                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                }

            }

            return result;
        }

        private bool saveEntryQLifeLiner(FourPsModel fpm)
        {
            bool result = false;

            if (isExistAccountNo(fpm.AccountNo))
                return result;

            using (SqlCommand cmd = new SqlCommand())
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();
                SqlTransaction trans;
                trans = con.BeginTransaction();
                try
                {

                    cmd.Connection = con;
                    cmd.Transaction = trans;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO [ebs].[dbo].[tbl_qualifiedFPQME]											   " +
                                      "           ([hh_id],[entryid],[consumerid],[dateapplied],[isQualified]				   " +
                                      "           ,[app_lname],[app_fname],[app_mname],[app_extname],[app_mdname]			   " +
                                      "           ,[app_gender],[app_addhouseno],[app_addstreet],[app_addbarangay]			   " +
                                      "           ,[app_addmunicipality],[app_addprovince],[app_addregion],[app_addpostal]	   " +
                                      "           ,[app_birthdate],[app_maritalstatus],[app_contactnumber],[ownership],[ownershipother] " +
                                      "           ,[validid],[valididno],[annualincome],[docchklst1],[docchklst2]			   " +
                                      "           ,[docchklst3],[docchklst4],[supportdocPOR],[supportdocLOA]				   " +
                                      "           ,[supportVGID],[supportSWDO],[evalisapproved],[reasonfordisapproved]		   " +
                                      "           ,[userid])																   " +
                                      "VALUES(@hh_id,@entryid,@consumerid,getdate(),@isQualified,@app_lname				   " +
                                      "	  ,@app_fname,@app_mname,@app_extname,@app_mdname,@app_gender,@app_addhouseno		   " +
                                      "	  ,@app_addstreet,@app_addbarangay,@app_addmunicipality,@app_addprovince			   " +
                                      "	  ,@app_addregion,@app_addpostal,@app_birthdate,@app_maritalstatus,@app_contactnumber  " +
                                      "	  ,@ownership,@ownershipother,@validid,@valididno,@annualincome,@docchklst1,@docchklst2,@docchklst3	   " +
                                      "	  ,@docchklst4,@supportdocPOR,@supportdocLOA,@supportVGID,@supportSWDO,@evalisapproved " +
                                      "	  ,@reasonfordisapproved,@userid)													   ";

                    cmd.Parameters.AddWithValue("@hh_id", fpm.HH_Id);
                    cmd.Parameters.AddWithValue("@entryid", fpm.EntryId);
                    cmd.Parameters.AddWithValue("@consumerid", fpm.AccountNo);
                    cmd.Parameters.AddWithValue("@isQualified", fpm.IsQualified);
                    cmd.Parameters.AddWithValue("@app_lname", fpm.Surname);
                    cmd.Parameters.AddWithValue("@app_fname", fpm.Givenname);
                    cmd.Parameters.AddWithValue("@app_mname", fpm.Middlename);

                    if(fpm.Extensionname==null)
                        cmd.Parameters.AddWithValue("@app_extname", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_extname", fpm.Extensionname);
                    if (fpm.Maidenname == null)
                        cmd.Parameters.AddWithValue("@app_mdname", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_mdname", fpm.Maidenname);
                    cmd.Parameters.AddWithValue("@app_gender", fpm.Gender);
                    if(fpm.HouseNumber==null)
                        cmd.Parameters.AddWithValue("@app_addhouseno", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addhouseno", fpm.HouseNumber);
                    if(fpm.Street==null)
                        cmd.Parameters.AddWithValue("@app_addstreet", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addstreet", fpm.Street);
                    if(fpm.Brgyname==null)
                        cmd.Parameters.AddWithValue("@app_addbarangay", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addbarangay", fpm.Brgyname);
                    if (fpm.Cityname == null)
                        cmd.Parameters.AddWithValue("@app_addmunicipality", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addmunicipality", fpm.Cityname);
                    if(fpm.Provname==null)
                        cmd.Parameters.AddWithValue("@app_addprovince", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addprovince", fpm.Provname);
                    if(fpm.Region==null)
                        cmd.Parameters.AddWithValue("@app_addregion", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addregion", fpm.Region);
                    if (fpm.Postal==null)
                        cmd.Parameters.AddWithValue("@app_addpostal",DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_addpostal", fpm.Postal);
                    
                    cmd.Parameters.AddWithValue("@app_birthdate", fpm.Birthday);

                    if(fpm.MaritalStatus==null)
                        cmd.Parameters.AddWithValue("@app_maritalstatus",DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@app_maritalstatus", fpm.MaritalStatus);
                    cmd.Parameters.AddWithValue("@app_contactnumber", fpm.ContactNo);
                    cmd.Parameters.AddWithValue("@ownership", fpm.Ownership);
                    if(fpm.OwnershipOther==null)
                        cmd.Parameters.AddWithValue("@ownershipother", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ownershipother", fpm.OwnershipOther);

                    cmd.Parameters.AddWithValue("@validid", fpm.ValidID);
                    cmd.Parameters.AddWithValue("@valididno", fpm.ValidIdNo);
                    cmd.Parameters.AddWithValue("@annualincome", fpm.AnnualIncome);
                    cmd.Parameters.AddWithValue("@docchklst1", fpm.DocCheckList1);
                    cmd.Parameters.AddWithValue("@docchklst2", fpm.DocCheckList2);
                    cmd.Parameters.AddWithValue("@docchklst3", fpm.DocCheckList3);
                    cmd.Parameters.AddWithValue("@docchklst4", fpm.DocCheckList4);
                    cmd.Parameters.AddWithValue("@supportdocPOR", fpm.SupportPOR);
                    cmd.Parameters.AddWithValue("@supportdocLOA", fpm.SupportLOA);
                    cmd.Parameters.AddWithValue("@supportVGID", fpm.SupportVGID);
                    cmd.Parameters.AddWithValue("@supportSWDO", fpm.SupportSWDO);
                    cmd.Parameters.AddWithValue("@evalisapproved", fpm.IsApproved);

                    if(fpm.ReasonForDisapproved==null)
                        cmd.Parameters.AddWithValue("@reasonfordisapproved", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@reasonfordisapproved", fpm.ReasonForDisapproved);
                    cmd.Parameters.AddWithValue("@userid", User.Identity.Name);

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

        private int getMaxIdOfUser(string userid)
        {
            int id = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select max(id) [currentid] " +
                                      "from tbl_mstrfourps " +
                                      "where userid=@userid";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userid", userid);

                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            id = Convert.ToInt32(rdr["currentid"]);
                        }
                    }


                }
                catch (Exception)
                {
                    id = 0;
                }
                finally
                {
                    cmd.Dispose();
                }

            }

            return id;
        }

        private DataTable getDataForReportForm(int reportid)
        {
            DataTable dt = new DataTable();



            return dt;
        }
    }
}