using Antlr.Runtime.Misc;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using System.IO;
using System.Web.Management;
using System.Security.Cryptography;
using Microsoft.Ajax.Utilities;
using PagedList;
using System.Reflection;

namespace TAR1ORMAN.Controllers
{
    public class MembersController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN")]
        // GET: Members
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllTowns()
        {
            List<TownModel> ltm = new List<TownModel>();
            ltm = getAllTowns();

            return Json(new { data = ltm }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadData(int twnid)
        {
            List<MembersModel> lstmm = new List<MembersModel>();
            lstmm = loadData(twnid);

            var jsonResult = Json(new { data = lstmm }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        public JsonResult GetMemberById(int id)
        {
            return Json(new { data = getMemberById(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllTypes()
        {
            List<MemberTypeModel> lstmtm = new List<MemberTypeModel>();
            lstmtm = getAllTypes();
            return Json(new { data = lstmtm }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNameAddressByAccountNo(string acctno)
        {
            ConsumerModel cm = new ConsumerModel();
            cm = getNameAddressByAccountNo(acctno);
            return Json(new { data = cm },JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllMemberAccountsById(int memberid)
        {
            return Json(new { data = getAllMemberAccountsById(memberid) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAccounts(List<MemberAccountModel> lstma)
        {
            bool result = deleteAccounts(lstma);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertNewAccounts(List<MemberAccountModel> lstma)
        {
            bool result = insertNewAccounts(lstma);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAccountAsPrimary(int memid, string acctno)
        {
            bool result = updateAccountAsPrimary(memid, acctno);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMemberDetails(MembersModel mem)
        {
            string result = string.Empty;
            return Json(result = updateMemberDetails(mem), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllMunicipalities()
        {
            return Json(new { data =getAllMunicipalities() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBarangaysByMunicipalityId(int id)
        {
            return Json(new { data = getAllBarangaysByMunicipalityId(id) }, JsonRequestBehavior.AllowGet);;
        }

        public JsonResult GetAllConsumerTypes()
        {
            return Json(new { data = getAllConsumerTypes() }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveNewMemberWithAccount(MemberWithAccountModel memacctm)
        {
            bool result = saveNewMemberAndAccount(memacctm);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //PROCEDURES AND FUNCTIONS
        private List<TownModel> getAllTowns()
        {
            List<TownModel> lsttm = new List<TownModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,displaytext[town] from tbloffices;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            lsttm.Add(new TownModel
                            {
                                Id = dr["id"].ToString(),
                                Town = dr["town"].ToString()
                            });
                        }
                        
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

        private List<MemberTypeModel> getAllTypes()
        {
            List<MemberTypeModel> lsttm = new List<MemberTypeModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id, type from b_membertypes;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lsttm.Add(new MemberTypeModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                MemberType = dr["type"].ToString()
                            });
                        }

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

        private List<MembersModel> loadData(int tid)
        {
            List<MembersModel> lstmm = new List<MembersModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                if (tid < 8)
                {
                    da.SelectCommand.CommandText = "select mem.id, case when isbusiness=1 then [businessname] else rtrim(lname) + ', ' + rtrim(fname) + ' ' + rtrim(isnull(mname,'')) + ' ' + rtrim(isnull(suffix,'')) end [name]," +
                                               "typ.type,isnull(memberid,'')memberid,isnull(memberdate,'')memberdate,barangay,municipality " +
                                               "from b_members mem inner join b_membertypes typ " +
                                               "on mem.membertypeid=typ.id " +
                                               "where officeid=@officeid";
                }
                else
                {
                    da.SelectCommand.CommandText = "select mem.id, case when isbusiness=1 then [businessname] else rtrim(lname) + ', ' + rtrim(fname) + ' ' + rtrim(isnull(mname,'')) + ' ' + rtrim(isnull(suffix,'')) end [name]," +
                                               "typ.type,isnull(memberid,'')memberid,isnull(memberdate,'')memberdate,barangay,municipality " +
                                               "from b_members mem inner join b_membertypes typ " +
                                               "on mem.membertypeid=typ.id;";
                }
                

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@officeid", tid);

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstmm.Add(new MembersModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                BusinessName = dr["name"].ToString(),
                                MemberType = dr["type"].ToString(),
                                MemberId = dr["memberid"].ToString(),
                                MemberDate = Convert.ToDateTime(dr["memberdate"]).ToShortDateString(),
                                Barangay = dr["barangay"].ToString(),
                                Municipality = dr["municipality"].ToString()
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lstmm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstmm;
        }

        private MembersModel getMemberById(int memid)
        {
            MembersModel mm = new MembersModel();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand()) 
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select mem.id, isbusiness,isnull(businessname,'')businessname, " + 
                                      "isnull(lname,'')lname,isnull(fname,'')fname,isnull(mname,'')mname,isnull(suffix,'')suffix, " +
                                      "mem.membertypeid,typ.type,isnull(memberid,'')memberid,isnull(memberdate,'')memberdate,barangay,municipality " +
                                      "from b_members mem inner join b_membertypes typ " +
                                      "on mem.membertypeid=typ.id where mem.id=@id;";

                    cmd.Parameters.AddWithValue("@id", memid);

                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            mm.Id = memid;
                            mm.BusinessName = rdr["businessname"].ToString().Trim();
                            mm.IsBusiness = Convert.ToBoolean(rdr["isbusiness"]);
                            mm.LastName = rdr["lname"].ToString();
                            mm.FirstName = rdr["fname"].ToString();
                            mm.MiddleName = rdr["mname"].ToString();
                            mm.Suffix = rdr["suffix"].ToString();
                            mm.MemberTypeId = Convert.ToInt32(rdr["membertypeid"]);
                            mm.MemberType = rdr["type"].ToString();
                            mm.MemberId = rdr["memberid"].ToString();
                            mm.MemberDate = Convert.ToDateTime(rdr["memberdate"]).ToString("yyyy-MM-dd");
                            mm.Barangay = rdr["barangay"].ToString();
                            mm.Municipality = rdr["municipality"].ToString();
                        }
                    }
                    else
                        mm = null;
                }
                
            }
            catch (Exception ex)
            {
                mm = null;
            }
            finally
            {
                con.Close();
            }

            return mm;
        }

        private ConsumerModel getNameAddressByAccountNo(string acctno)
        {
            ConsumerModel cm = new ConsumerModel();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());

            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select consumerid,name,address " +
                                      "from arsconsumer " +
                                      "where consumerid=@consumerid;";

                    cmd.Parameters.AddWithValue("@consumerid", acctno);

                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            cm.AccountNo = rdr["consumerid"].ToString();
                            cm.AccountName = rdr["name"].ToString();
                            cm.Address = rdr["address"].ToString();
                        }
                    }
                    else
                        cm = null;
                }

            }
            catch (Exception ex)
            {
                cm = null;
            }
            finally
            {
                con.Close();
            }

            return cm;
        }

        private List<MemberAccountModel> getAllMemberAccountsById(int memid)
        {
            List<MemberAccountModel> lstmm = new List<MemberAccountModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;

                da.SelectCommand.CommandText = "select a.id,a.memberid,accountno,b.address,isprimary " +
                                               "from b_memberaccounts a inner join arsconsumer b " +
                                               "on a.accountno=b.consumerid " +
                                               "where a.memberid=@memberid;";

                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.AddWithValue("@memberid", memid);

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstmm.Add(new MemberAccountModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                MemberId = Convert.ToInt32(dr["memberid"]),
                                AccountNo = dr["accountno"].ToString(),
                                Address = dr["address"].ToString(),
                                IsPrimary = Convert.ToBoolean(dr["isprimary"])
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lstmm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstmm;
        }

        private bool deleteAccounts(List<MemberAccountModel> lstmam)
        {
            bool result = true;

            if (lstmam.Count > 0)
            {

                foreach (MemberAccountModel mam in lstmam)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                        con.Open();

                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "delete from b_memberaccounts " +
                                          "where memberid=@memberid and accountno=@accountno; ";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@memberid", mam.MemberId);
                        cmd.Parameters.AddWithValue("@accountno", mam.AccountNo);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                        finally
                        {
                            cmd.Dispose();
                            con.Close();
                        }
                    }
                }
            }

            return result;
        }

        private bool insertNewAccounts(List<MemberAccountModel> lstmam)
        {
            bool result = true;

            foreach (MemberAccountModel mam in lstmam)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                    con.Open();

                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "insert into b_memberaccounts(memberid,accountno,isprimary) " +
                                      "values(@memberid,@accountno,@isprimary)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@memberid", mam.MemberId);
                    cmd.Parameters.AddWithValue("@accountno", mam.AccountNo);
                    cmd.Parameters.AddWithValue("@isprimary", mam.IsPrimary);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        cmd.Dispose();
                        con.Close();
                    }
                }
            }

            return result;
        }

        private bool updateAccountAsPrimary(int memberid, string selacctno)
        {
            bool result = true;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "update b_memberaccounts set isprimary=0 where memberid=@memberid;" +
                                  "update b_memberaccounts set isprimary=1 where memberid=@memberid and accountno=@accountno;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@memberid", memberid);
                cmd.Parameters.AddWithValue("@accountno", selacctno);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        private string updateMemberDetails(MembersModel mm)
        {
            string msgresult = string.Empty;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                if(mm.MemberTypeId==1 || mm.MemberTypeId==2)
                    cmd.CommandText = "update b_members set isbusiness=0,lname=@lname,fname=@fname,mname=@mi,suffix=@suffix," +
                                      "businessname=NULL,membertypeid=@membertypeid,barangay=@barangay,municipality=@municipality," +
                                      "memberid=@memberid,memberdate=@memberdate,lastupdated=getdate(),updatedby=@updatedby " +
                                      "where id=@memid";
                else // JURIDICAL
                    cmd.CommandText = "update b_members set isbusiness=1,lname=NULL,fname=NULL,mname=NULL,suffix=NULL," +
                                      "businessname=@businessname,membertypeid=@membertypeid,barangay=@barangay,municipality=@municipality," +
                                      "memberid=@memberid,memberdate=@memberdate,lastupdated=getdate(),updatedby=@updatedby " +
                                      "where id=@memid";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@businessname", mm.BusinessName is null ? string.Empty : mm.BusinessName);
                cmd.Parameters.AddWithValue("@lname", mm.LastName is null ? string.Empty : mm.LastName);
                cmd.Parameters.AddWithValue("@fname", mm.FirstName is null ? string.Empty : mm.FirstName);
                cmd.Parameters.AddWithValue("@mi", mm.MiddleName is null ? string.Empty : mm.MiddleName);
                cmd.Parameters.AddWithValue("@suffix", mm.Suffix is null ? string.Empty : mm.Suffix);
                cmd.Parameters.AddWithValue("@membertypeid", mm.MemberTypeId);
                cmd.Parameters.AddWithValue("@barangay", mm.Barangay);
                cmd.Parameters.AddWithValue("@municipality", mm.Municipality);
                cmd.Parameters.AddWithValue("@memberid", mm.MemberId);
                cmd.Parameters.AddWithValue("@memberdate", mm.MemberDate);
                cmd.Parameters.AddWithValue("@updatedby", User.Identity.Name);
                cmd.Parameters.AddWithValue("@memid", mm.Id);

                try
                {
                    cmd.ExecuteNonQuery();
                    msgresult = "Updated Successfully.";
                }
                catch (Exception ex)
                {
                    msgresult = "An error occured: \n" + ex.Message;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return msgresult;
        }

        private List<MunicipalityProvinceModel> getAllMunicipalities()
        {
            List<MunicipalityProvinceModel> lstmpm = new List<MunicipalityProvinceModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,municipality, province from b_municipalities;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstmpm.Add(new MunicipalityProvinceModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Municipality = dr["municipality"].ToString(),
                                Province = dr["province"].ToString()
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lstmpm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstmpm;
        }

        private List<BarangayModel> getAllBarangaysByMunicipalityId(int id)
        {
            List<BarangayModel> lstbm = new List<BarangayModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,UPPER(barangay) barangay from b_barangays where municipalityid=@munid;";

                da.SelectCommand.Parameters.AddWithValue("@munid", id);
                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstbm.Add(new BarangayModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Barangay = dr["barangay"].ToString()
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    lstbm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstbm;
        }

        private List<ConsumerTypeModel> getAllConsumerTypes()
        {
            List<ConsumerTypeModel> lstctm = new List<ConsumerTypeModel>();

            DataTable dt = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select RTRIM(consumertypeid)[code],upper(RTRIM(description))[description] from arstype;";

                try
                {
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstctm.Add(new ConsumerTypeModel
                            {
                                Id = dr["code"].ToString(),
                                ConsumerType = dr["description"].ToString()
                            });
                        }

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

        //private bool addMemberWithNonExistAccount(MembersModel member, MemberAccountModel macct)
        //{

        //}

        private bool saveNewMemberAndAccount(MemberWithAccountModel mwam)
        {
            bool result = true;

            string usr = User.Identity.Name.ToString();

            string[] municipal = mwam.Municipality.Split(',');
            mwam.Municipality = municipal[0];

            if (mwam.FName == null)
                mwam.FName = "";

            if (mwam.LName == null)
                mwam.LName = "";

            if (mwam.MName == null)
                mwam.MName = "";

            if (mwam.Suffix == null)
                mwam.Suffix = "";

            if (mwam.BusinessName == null)
                mwam.BusinessName = "";

            if (mwam.ConsumerType == null)
                mwam.ConsumerType = "";

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_NewMemWithAcct";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@isbusiness", mwam.IsBusiness);
                cmd.Parameters.AddWithValue("@lname", mwam.LName.ToUpper());
                cmd.Parameters.AddWithValue("@fname", mwam.FName.ToUpper());
                cmd.Parameters.AddWithValue("@mname", mwam.MName.ToUpper());
                cmd.Parameters.AddWithValue("@suffix", mwam.Suffix);
                cmd.Parameters.AddWithValue("@businessname", mwam.BusinessName.ToUpper());
                cmd.Parameters.AddWithValue("@membertypeid", mwam.MemberTypeId);
                cmd.Parameters.AddWithValue("@barangay", mwam.Barangay.ToUpper());
                cmd.Parameters.AddWithValue("@municipality", mwam.Municipality.ToUpper());
                cmd.Parameters.AddWithValue("@officeid", mwam.OfficeId);
                cmd.Parameters.AddWithValue("@memberid", mwam.MemberId);
                cmd.Parameters.AddWithValue("@memberdate", mwam.MemberDate);
                cmd.Parameters.AddWithValue("@madeby", usr);
                cmd.Parameters.AddWithValue("@accountno", mwam.AccountNo);
                cmd.Parameters.AddWithValue("@consumertype", mwam.ConsumerType);
                cmd.Parameters.AddWithValue("@isexistacct", mwam.IsExistAccount);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

    }
}