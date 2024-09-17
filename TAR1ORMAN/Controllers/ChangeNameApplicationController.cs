using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using TAR1ORDATA.DataModel;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace TAR1ORMAN.Controllers
{
    public class ChangeNameApplicationController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: ChangeName
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAccountDetails(string acctnum)
        {
            ConsumerModel cmdtl = new ConsumerModel();
            cmdtl = getAccountDetailsByAcctNum(acctnum);

            return Json(cmdtl, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertNewApplicant(ChangeNameModel cnm)
        {
            cnm.MadeById = User.Identity.Name;
            //cnm.MadeById = "1020"; //for development only. Need to change.
            return Json(insertNewApplicant(cnm), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreviewChangeNameApplicationReport()
        {
            int changenameid = getMaxChangeNameIdByUser(User.Identity.Name);

            //initialize table for report
            DataTable tblSource = new DataTable();
            tblSource = getChangeNameDetails(changenameid);

            LocalReport lr = new LocalReport();
            string p = string.Empty;
            
            if (Convert.ToInt32(tblSource.Rows[0]["ChangeType"]) == 1)
                p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameWithNew.rdlc");
            else {
                if (Convert.ToInt32(tblSource.Rows[0]["ChangeType"]) == 2)
                    p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameWithOldNew.rdlc");
                else
                {
                    if (Convert.ToInt32(tblSource.Rows[0]["ChangeType"]) == 3)
                        p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameReten.rdlc");
                    else
                    {
                        p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameRetenNew.rdlc");
                    }
                }
                    
            }

            lr.ReportPath = p;

            //ReportDataSource for Header
            ReportDataSource rptdatasrc = new ReportDataSource("dsChangeName", tblSource);

            lr.DataSources.Add(rptdatasrc);

            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }

            //functions and procedures
        private ConsumerModel getAccountDetailsByAcctNum(string acctnum)
        {
            ConsumerModel cm = new ConsumerModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select consumerid, rtrim(name)[name], rtrim(address)[address], rtrim(isnull(memberid,''))[memberid], " +
                                  "rtrim(mtrserialno)[mtrserialno], memberdate,seqno " +
                                  "from arsconsumer where consumerid=@consumerid";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            cm.AccountNo = acctnum;
                            cm.AccountName = rdr["name"].ToString();
                            cm.Address = rdr["address"].ToString();
                            if (rdr["memberid"].ToString() == "")
                                cm.MemberId = null;
                            else
                                cm.MemberId = rdr["memberid"].ToString();
                            cm.MeterNo = rdr["mtrserialno"].ToString();
                            if (rdr["memberdate"].ToString() == "")
                                cm.ORDate = null;
                            else
                                cm.ORDate = Convert.ToDateTime(rdr["memberdate"]).ToString("yyyy-MM-dd");
                            cm.SeqNo = rdr["seqno"].ToString();
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
                    con.Close();
                }
            }

            return cm;
        }

        private bool insertNewApplicant(ChangeNameModel pcnm)
        {
            bool result = false;

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
                    cmd.CommandText = "declare @OLD_MEMBERID varchar(10), @OLD_MEMBERDATE datetime; " +
                                      "insert into tbl_changename(appdate,accountno,old_name,old_memberid,old_memberdate,nw_name,nw_memberid,  " +
                                      "nw_memberdate,nw_birthday,nw_contactnum,nw_relationship,nw_reason,changenametypeid,madeby,isdied,  " +
                                      "isremdeathcert,isremauthletter,isremdeedofsale,isremletterofreq,isremother,remothertext,rptremark,appstatus)  " +
                                      "values(getdate(),@accountno,@oldname,@oldmemberid,@oldmemberdate,@nw_name,null, " +
                                      "null,@nw_birthday,@nw_contactnum,@nw_relationship,@nw_reason,@changenametypeid, " +
                                      "@madeby,@isdied,@isremdeathcert,@isremauthletter,@isremdeedofsale,@isremletterofreq,@isremother,@remothertext,@rptremark,'FOR PAYMENT'); " +
                                      "SELECT @OLD_MEMBERID=RTRIM(memberid), @OLD_MEMBERDATE=CASE WHEN ISDATE(CAST(memberdate AS VARCHAR))=1 THEN memberdate ELSE NULL END FROM arsconsumer WHERE consumerid=@accountno;  " +
                                      "IF @OLD_MEMBERID = ''  " +
                                      "BEGIN  " +
                                      "     UPDATE arsconsumer  " +
                                      "     SET memberid = @oldmemberid  " +
                                      "     WHERE consumerid = @accountno;  " +
                                      "END  " +
                                      "IF @OLD_MEMBERDATE IS NULL  " +
                                      "BEGIN  " +
                                      "     UPDATE arsconsumer  " +
                                      "     SET memberdate = @oldmemberdate  " +
                                      "     WHERE consumerid = @accountno;  " +
                                      "END ";

                    cmd.Parameters.AddWithValue("@accountno", pcnm.AccountNo);
                    cmd.Parameters.AddWithValue("@oldname", pcnm.AccountName);

                    if (pcnm.MadeById == null)
                        cmd.Parameters.AddWithValue("@oldmemberid", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@oldmemberid", pcnm.MemberId);

                    if (pcnm.MemberDate == null)
                        cmd.Parameters.AddWithValue("@oldmemberdate", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@oldmemberdate", pcnm.MemberDate);

                    cmd.Parameters.AddWithValue("@nw_name", pcnm.NewName);

                    cmd.Parameters.AddWithValue("@nw_birthday", pcnm.Birthday);

                    if(pcnm.ContactNo==null)
                        cmd.Parameters.AddWithValue("@nw_contactnum", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@nw_contactnum", pcnm.ContactNo);

                    cmd.Parameters.AddWithValue("@nw_relationship", pcnm.Relationship);
                    cmd.Parameters.AddWithValue("@nw_reason", pcnm.Reason);
                    //cmd.Parameters.AddWithValue("@nw_forwithdrawold", pcnm.ForWithdrawOld);
                    //cmd.Parameters.AddWithValue("@nw_forwithdrawnew", pcnm.ForWithdrawNew);
                    //cmd.Parameters.AddWithValue("@nw_forretention", pcnm.ForRetention);
                    cmd.Parameters.AddWithValue("@changenametypeid", pcnm.ChangeNameTypeId);
                    cmd.Parameters.AddWithValue("@madeby", pcnm.MadeById);
                    cmd.Parameters.AddWithValue("@isdied", pcnm.IsDied);
                    //cmd.Parameters.AddWithValue("@remarks", pcnm.Remarks);
                    //@isremdeathcert,@isremauthletter,@isremdeedofsale,@isremletterofreq,@isremother,@remothertext
                    cmd.Parameters.AddWithValue("@isremdeathcert", pcnm.IsRemDeathCert);
                    cmd.Parameters.AddWithValue("@isremauthletter", pcnm.IsRemAuthLetter);
                    cmd.Parameters.AddWithValue("@isremdeedofsale", pcnm.IsRemDeedOfSale);
                    cmd.Parameters.AddWithValue("@isremletterofreq", pcnm.IsRemLetterOfReq);
                    cmd.Parameters.AddWithValue("@isremother", pcnm.IsRemOther);
                    if(pcnm.RemOtherText==null)
                        cmd.Parameters.AddWithValue("@remothertext", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@remothertext", pcnm.RemOtherText);
                    cmd.Parameters.AddWithValue("@rptremark", pcnm.RptRemark);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    result = true;
                }
                catch(Exception ex)
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

        private int getMaxChangeNameIdByUser(string userid)
        {
            int id = 0;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select max(id)[id] " +
                                  "from tbl_changename where madeby=@madeby";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@madeby", userid);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    while (rdr.Read())
                    {
                        if (rdr.HasRows)
                        {
                            id = Convert.ToInt32(rdr["id"]);
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
                    con.Close();
                }
            }

            return id;
        }

        private DataTable getChangeNameDetails(int id)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select id[Id],appdate[ApplicationDate],accountno[AccountNo],UPPER(rtrim(b.name))[AccountName]," +
                                  "UPPER(rtrim(address))[AccountAddress],memberid[MemberId],memberdate[MemberDate],mtrserialno[MeterNo],seqno[SequenceNo]," +
                                  "nw_name[NewName],nw_memberid[NewMemberId],nw_memberdate[NewMemberDate],nw_birthday[Birthday],nw_contactnum[ContactNo]," +
                                  "nw_relationship[Relationship],nw_reason[Reason],changenametypeid[ChangeType] " +
                                  "from tbl_changename a inner join arsconsumer b " +
                                  "on a.accountno = b.consumerid " +
                                  "where a.id = @id";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    da.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    dtResult = null;
                }
                finally
                {
                    da.Dispose();
                    cmd.Dispose();
                    con.Close();
                }
            }

            return dtResult;
        }
    }
}