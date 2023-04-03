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
            
            if (tblSource.Rows[0]["ChangeType"].ToString() == "Retention")
                p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameReten.rdlc");
            else {
                if (tblSource.Rows[0]["ChangeType"].ToString() == "Old")
                    p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameWithOld.rdlc");
                else
                    p = Path.Combine(Server.MapPath("/Reports"), "rptChangeNameWithNew.rdlc");
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
                                  "rtrim(mtrserialno)[mtrserialno], case when memberdate is null then '' else convert(varchar(10),memberdate,101) end [memberdate],seqno " +
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
                            cm.MemberId = rdr["memberid"].ToString();
                            cm.MeterNo = rdr["mtrserialno"].ToString();
                            cm.ORDate = rdr["memberdate"].ToString();
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
                    cmd.CommandText = "insert into tbl_changename(appdate,accountno,nw_name,nw_memberid," +
                                      "nw_memberdate,nw_birthday,nw_contactnum,nw_relationship,nw_reason," +
                                      "nw_forwithdrawold,nw_forwithdrawnew,nw_forretention,madeby) " +
                                      "values(getdate(),@accountno,@nw_name,@nw_memberid," +
                                      "@nw_memberdate,@nw_birthday,@nw_contactnum,@nw_relationship,@nw_reason," +
                                      "@nw_forwithdrawold,@nw_forwithdrawnew,@nw_forretention,@madeby);";

                    cmd.Parameters.AddWithValue("@accountno", pcnm.AccountNo);
                    cmd.Parameters.AddWithValue("@nw_name", pcnm.NewName);
                    cmd.Parameters.AddWithValue("@nw_memberid", pcnm.NewMemberId);
                    cmd.Parameters.AddWithValue("@nw_memberdate", pcnm.NewMemberDate);
                    cmd.Parameters.AddWithValue("@nw_birthday", pcnm.Birthday);
                    cmd.Parameters.AddWithValue("@nw_contactnum", pcnm.ContactNo);
                    cmd.Parameters.AddWithValue("@nw_relationship", pcnm.Relationship);
                    cmd.Parameters.AddWithValue("@nw_reason", pcnm.Reason);
                    cmd.Parameters.AddWithValue("@nw_forwithdrawold", pcnm.ForWithdrawOld);
                    cmd.Parameters.AddWithValue("@nw_forwithdrawnew", pcnm.ForWithdrawNew);
                    cmd.Parameters.AddWithValue("@nw_forretention", pcnm.ForRetention);
                    cmd.Parameters.AddWithValue("@madeby", pcnm.MadeById);

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
                                  "nw_relationship[Relationship],nw_reason[Reason]," +
                                  "case when isnull(nw_forwithdrawold,0)= 0 then " +
                                  "     case when isnull(nw_forwithdrawnew,0)= 0 then " +
                                  "             'Retention' " +
                                  "     else " +
                                  "             'New' " +
                                  "     end " +
                                  "else " +
                                  "     'Old' " +
                                  "end[ChangeType] " +
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