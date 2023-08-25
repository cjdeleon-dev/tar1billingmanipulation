using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
            fpm.EntryUserId = User.Identity.Name;

            if (saveEntryQLifeLiner(fpm))
                result = true;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PreviewQualifiedLifelinerReport()
        {
            //initialize parameters
            DataTable dtData = getDataForReportForm(getMaxIdOfUser(User.Identity.Name));

            string p = string.Empty;

            LocalReport lr = new LocalReport();

            if (Convert.ToBoolean(dtData.Rows[0]["IsQualified"]))
                p = Path.Combine(Server.MapPath("/Reports"), "rptFourPsForm.rdlc");
            else
                p = Path.Combine(Server.MapPath("/Reports"), "rptQMEForm.rdlc");

            lr.ReportPath = p;

            //ReportDataSource
            ReportDataSource rptds = new ReportDataSource("dsFPQME", dtData);

            //Add datasource to bind
            lr.DataSources.Add(rptds);
            
            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public JsonResult GetAccountDetails(string accountno)
        {
            return Json(new { data = getAccountDetails(accountno) }, JsonRequestBehavior.AllowGet);
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
                                      "last_name,first_name,middle_name,ext_name,birthday,upper(sex)[sex] " +
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

        private bool isExistAccountNo(string accountno,string entryid)
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
                    if(entryid!=null)
                        cmd.CommandText = "select isnull(count(*),0) numrows " +
                                      "from tbl_qualifiedFPQME " +
                                      "where consumerid = @consumerid or entryid=@entryid;";
                    else
                        cmd.CommandText = "select isnull(count(*),0) numrows " +
                                      "from tbl_qualifiedFPQME " +
                                      "where consumerid = @consumerid;";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@consumerid", accountno);
                    if(entryid!=null)
                        cmd.Parameters.AddWithValue("@entryid", entryid);

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

            if (isExistAccountNo(fpm.AccountNo,fpm.EntryId))
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
                                      "           ,[certificationno],[validid],[valididno],[annualincome],[docchklst1],[docchklst2]			   " +
                                      "           ,[docchklst3],[docchklst4],[supportdocPOR],[supportdocLOA]				   " +
                                      "           ,[supportVGID],[supportSWDO],[evalisapproved],[reasonfordisapproved]		   " +
                                      "           ,[userid])																   " +
                                      "VALUES(@hh_id,@entryid,@consumerid,getdate(),@isQualified,@app_lname				   " +
                                      "	  ,@app_fname,@app_mname,@app_extname,@app_mdname,@app_gender,@app_addhouseno		   " +
                                      "	  ,@app_addstreet,@app_addbarangay,@app_addmunicipality,@app_addprovince			   " +
                                      "	  ,@app_addregion,@app_addpostal,@app_birthdate,@app_maritalstatus,@app_contactnumber  " +
                                      "	  ,@ownership,@ownershipother,@certificationno,@validid,@valididno,@annualincome,@docchklst1,@docchklst2,@docchklst3	   " +
                                      "	  ,@docchklst4,@supportdocPOR,@supportdocLOA,@supportVGID,@supportSWDO,@evalisapproved " +
                                      "	  ,@reasonfordisapproved,@userid)													   ";

                    if(fpm.HH_Id==null)
                        cmd.Parameters.AddWithValue("@hh_id", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@hh_id", fpm.HH_Id);

                    if(fpm.EntryId==null)
                        cmd.Parameters.AddWithValue("@entryid", DBNull.Value);
                    else
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
                    if(fpm.CertificationNo==null)
                        cmd.Parameters.AddWithValue("@certificationno", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@certificationno", fpm.CertificationNo);


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
                    cmd.Parameters.AddWithValue("@userid", fpm.EntryUserId);

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
                                      "from tbl_qualifiedFPQME " +
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
                catch (Exception ex)
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

            using (SqlDataAdapter da = new SqlDataAdapter())
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                try
                {

                    da.SelectCommand = new SqlCommand("select CASE WHEN a.isQualified=1 THEN 'LL'+CAST(YEAR(GETDATE())AS varchar)+'-'+RIGHT('0000000000'+cast(a.id as varchar),10) " +
                                                      "ELSE 'ME'+CAST(YEAR(GETDATE())AS varchar)+'-'+RIGHT('0000000000'+cast(a.id as varchar),10) END [ApplicationNo]," +
                                                      "a.id[Id], a.hh_id[HouseHoldId],a.entryid[EntryId], RTRIM(a.consumerid + ' - ' + b.[name])[AccountNo],a.dateapplied[DateApplied]," +
                                                      "a.isQualified[IsQualified], RTRIM(a.app_lname)[LastName],RTRIM(a.app_fname)[FirstName], RTRIM(a.app_mname)[MiddleName], a.app_extname[ExtensionName]," +
                                                      "a.app_mdname[MaidenName], RTRIM(a.app_gender)[Gender], a.app_addhouseno[HouseNumber],a.app_addstreet[Street], a.app_addbarangay[Barangay]," +
                                                      "a.app_addmunicipality[Municipality],a.app_addprovince[Province], a.app_addregion[Region], a.app_addpostal[Postal]," +
                                                      "a.app_birthdate[Birthdate], a.app_maritalstatus[MaritalStatus], '+63' + a.app_contactnumber[ContactNo],a.ownership[Ownership]," +
                                                      "a.ownershipother[OwnershipOther],a.certificationno[CertificationNo], a.validid[ValidId],a.valididno[ValidIdNo], a.annualincome[AnnualIncome]," +
                                                      "a.docchklst1[CheckList1], a.docchklst2[CheckList2], a.docchklst3[CheckList3], a.docchklst4[CheckList4]," +
                                                      "a.supportdocPOR[CheckPOR], a.supportdocLOA[CheckLOA], a.supportVGID[CheckVGID], a.supportSWDO[CheckSWDO]," +
                                                      "a.evalisapproved[Approved], a.reasonfordisapproved[DisReason], a.userid[UserId], UPPER(RTRIM(c.name))[UserName] " +
                                                      "from tbl_qualifiedFPQME a inner join arsconsumer b " +
                                                      "on a.consumerid = b.consumerid " +
                                                      "inner join secuser c " +
                                                      "on a.userid = c.userid " +
                                                      "where id = @id");
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.Parameters.AddWithValue("@id", reportid);

                    da.SelectCommand.Connection = con;

                    da.Fill(dt);
                    
                }
                catch (Exception ex)
                {
                    dt = null;
                }
                finally
                {
                    con.Close();
                }

            }

            return dt;
        }

        private ConsumerModel getAccountDetails(string accountnumber)
        {
            ConsumerModel cm = new ConsumerModel();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select consumerid, name, address " +
                                      "from arsconsumer " +
                                      "where consumerid = @consumerid";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@consumerid", accountnumber);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            cm.AccountNo = rdr["consumerid"].ToString();
                            cm.AccountName = rdr["name"].ToString();
                            cm.Address = rdr["address"].ToString();
                        }
                    }


                }
                catch (Exception ex)
                {
                    cm = null;
                }
                finally
                {
                    cmd.Dispose();
                }

            }

            return cm;
        }
    }
}