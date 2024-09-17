using iTextSharp.text.pdf;
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
    public class BurialApplicationValidationController : Controller
    {
        private static int headerid = 0;

        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]

        // GET: BurialAssistanceValidation
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

        public JsonResult NoUnsettledObligation(string acctnum)
        {
            bool resVal = false;
            resVal = noUnsettledObligation(acctnum);
            return Json(resVal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NoApprehended(string acctnum)
        {
            bool resVal = false;
            resVal = noApprehended(acctnum);
            return Json(resVal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InAvgConsumption(string acctnum)
        {
            bool resVal = false;
            resVal = inAvg150KWhConsumption(acctnum);
            return Json(resVal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NotClaimed(string acctnum)
        {
            bool resVal = false;
            resVal = notClaimedBurial(acctnum);
            return Json(resVal, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBurialResult(string acctnum)
        {
            bool res = false;

            string userid = User.Identity.Name;
            int hdrid = 0;

            bool itm1, itm2, itm3, itm4 = false;
            string res1=string.Empty, res2=string.Empty, res3=string.Empty, res4 = string.Empty;

            itm1 = noUnsettledObligation(acctnum);
            itm2 = noApprehended(acctnum);
            itm3 = inAvg150KWhConsumption(acctnum);
            itm4 = notClaimedBurial(acctnum);

            if (itm1 == false)
                //num of months
                res1 = getNumOfMonths(acctnum);
            else res1="0";

            if (itm2 == false)
                //get apprehended date
                res2 = getApprehendedDate(acctnum);
            else res2 = string.Empty;
            //avg KWH
            res3 = getOneYearAVGKWh(acctnum);

            if (itm4 == false)
                //get date claimed burial
                res4 = getClaimedDate(acctnum);
            else res4 = string.Empty;


            if (userid.Trim() != "")
            {
                //save burial result header
                if (insertResultHdr(acctnum, userid))
                {
                    hdrid = getIdNewInsertedResult(userid);
                    headerid = hdrid;
                    if (insertResultDtl(hdrid, itm1, itm2, itm3, itm4, res1, res2, res3, res4))
                        res = true;
                }
            }
            
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveBurialApplication(BurialClaimantModel bcm)
        {
            bool res = false;

            string userid = User.Identity.Name;
            bcm.BurialHeaderId = getIdNewInsertedResult(userid);
            res = insertClaimantDetails(bcm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintBurialApplicationResult()
        {
            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptBurialVerificationResult.rdlc");
            lr.ReportPath = p;

            //initialize parameters

            DataTable dtHeaderData = getHeaderResult(headerid) ;
            DataTable dtDetailsData = getDetailResult(headerid);
            //added by CJ 09/13/2022
            DataTable dtClaimantData = getClaimantData(headerid);

            //ReportDataSource for Header
            ReportDataSource bvrhdr = new ReportDataSource("dsBurialResultHdr", dtHeaderData);
            ReportDataSource bvrdtl = new ReportDataSource("dsBurialResultDtl", dtDetailsData);
            //added by CJ 09/13/2022
            ReportDataSource bvrclm = new ReportDataSource("dsBurialClaimant", dtClaimantData);

            lr.DataSources.Add(bvrhdr);//Header
            lr.DataSources.Add(bvrdtl);//Deatails
            //added by CJ 09/13/2022
            lr.DataSources.Add(bvrclm);//Claimant details

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
                                  "rtrim(mtrserialno)[mtrserialno], memberdate " +
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
                            if (rdr["memberdate"].ToString() != "")
                                cm.ORDate = Convert.ToDateTime(rdr["memberdate"].ToString()).ToString("yyyy-MM-dd");
                            else
                                cm.ORDate = "";
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

        private bool noUnsettledObligation(string acctnum)
        {
            bool result = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select sum(trxbalance) balance from arstrxhdr hdr inner join arstrxdtl dtl on hdr.trxseqid=dtl.trxseqid where hdr.consumerid=@consumerid";

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
                            if (Convert.ToDouble(rdr["balance"]) == 0)
                                result = true;
                            else
                                result = false;
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
                    con.Close();
                }
            }

            return result;
        }

        private bool noApprehended(string acctnum)
        {
            bool result = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select trxdate from arsstatuslog where consumerid=@consumerid and (oldstatusid='X' or newstatusid='X');";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    if (rdr.HasRows)
                        result = false;
                    else
                        result = true;

                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        private bool inAvg150KWhConsumption(string acctnum)
        {
            bool result = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "declare @tblAvgKWh as table(energyused numeric(8,2));" +
                                  "insert into @tblAvgKWh " +
                                  "select top 12 CAST(energyused as numeric(8,2)) " +
                                  "from arstrxhdr " +
                                  "where consumerid = @consumerid and trxid='EB' " +
                                  "order by trxdate desc; " +
                                  "select AVG(energyused) averageKWh from @tblAvgKWh; ";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            if (Convert.ToDouble(rdr["averageKWh"]) > 150.00)
                                result = false;
                            else
                                result = true;
                        }
                    }
                    else
                        result = false;

                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        private bool notClaimedBurial(string acctnum)
        {
            bool result = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select isnull(isclaimburial,'False') isclaimburial from arsconsumer where consumerid=@consumerid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            if (Convert.ToBoolean(rdr["isclaimburial"]))
                                result = false;
                            else
                                result = true;
                        }
                    }
                    else
                        result = false;

                }
                catch (Exception ex)
                {
                    result = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return result;
        }

        private bool insertResultHdr(string acctnum, string userid)
        {
            bool resval = false;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "insert into tbl_burialresulthdr(dategen,userid,consumerid) " + 
                                  "values(getdate(),@userid,@consumerid)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@consumerid", acctnum);

                try
                {
                    cmd.ExecuteNonQuery();
                    resval = true;
                }
                catch (Exception ex)
                {
                    resval = false;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return resval;
        }

        private int getIdNewInsertedResult(string userid)
        {
            int retVal = 0;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select max(id) [newid] from tbl_burialresulthdr where userid=@userid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userid", userid);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            retVal = Convert.ToInt32(rdr["newid"]);
                        }
                    }

                }
                catch (Exception ex)
                {
                    retVal  = 0;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }

            return retVal;
        }

        private bool insertResultDtl(int hdrid, bool item1, bool item2, bool item3, bool item4, 
            string res1, string res2, string res3, string res4)
        {
            bool result = true;

            for(int i=1; i<=4; i++)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                    con.Open();

                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "insert into tbl_burialresultdtl(burialresulthdrid,burialfindid,resultval,result) " +
                                      "values(@hdrid,@bfid,@resultval,@result)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@hdrid", hdrid);
                    cmd.Parameters.AddWithValue("@bfid", i);

                    switch (i)
                    {
                        case 1:
                            cmd.Parameters.AddWithValue("@result", item1);
                            cmd.Parameters.AddWithValue("@resultval", res1);
                            break;
                        case 2:
                            cmd.Parameters.AddWithValue("@result", item2);
                            cmd.Parameters.AddWithValue("@resultval", res2);
                            break;
                        case 3:
                            cmd.Parameters.AddWithValue("@result", item3);
                            cmd.Parameters.AddWithValue("@resultval", res3);
                            break;
                        default:
                            cmd.Parameters.AddWithValue("@result", item4);
                            cmd.Parameters.AddWithValue("@resultval", res4);
                            break;
                    }
                    

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return result = false;
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

        private DataTable getHeaderResult(int hdrid)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select hdr.id[Id],hdr.dategen[GenDate],usr.name [VerifiedBy],hdr.consumerid[AccountNo],cons.name[AccountName]," +
                                  "cons.address[Address],typ.description [Type],cons.memberid[MemberORNo],cons.memberdate[DateMemberOR],mtrserialno[MeterSerialNo] " +
                                  "from tbl_burialresulthdr hdr inner " +
                                  "join arsconsumer cons " +
                                  "on hdr.consumerid = cons.consumerid " +
                                  "inner join secuser usr " +
                                  "on hdr.userid = usr.userid " +
                                  "inner join arstype typ " +
                                  "on cons.consumertypeid = typ.consumertypeid " +
                                  "where hdr.id = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    dt = null;
                }
                finally
                {
                    da.Dispose();
                    cmd.Dispose();
                    con.Close();
                }
            }

            return dt;
        }

        private DataTable getDetailResult(int hdrid)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select dtl.id[Id],fnd.itemdesc[Findings],dtl.result[Result],dtl.burialfindid[FindingId],dtl.resultval [ResultVal] " +
                                  "from tbl_burialresultdtl dtl inner " +
                                  "join tbl_burialfindings fnd " +
                                  "on dtl.burialfindid = fnd.id " +
                                  "where burialresulthdrid = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    dt = null;
                }
                finally
                {
                    da.Dispose();
                    cmd.Dispose();
                    con.Close();
                }
            }

            return dt;
        }

        private string getNumOfMonths(string acctnum)
        {
            string str = string.Empty;
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select count(consumerid) [ctr] from arstrxhdr hdr where hdr.consumerid=@consumerid and (trxbalance>0 or vatbalance>0) and trxid='EB'";

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
                            str = rdr["ctr"].ToString();
                        }
                        else str = (0).ToString();
                    }
                }
                catch (Exception ex)
                {
                    str = (0).ToString();
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }
            return str;
        }

        private string getApprehendedDate(string acctnum)
        {
            string str = string.Empty;
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select ISNULL(cast(min(trxdate) as varchar(10)),'') [appdate] from arsstatuslog where consumerid=@consumerid and (oldstatusid='X' or newstatusid='X');;";

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
                            str = rdr["appdate"].ToString()==""?string.Empty: rdr["appdate"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    str = string.Empty;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }
            return str;
        }

        private string getOneYearAVGKWh(string acctnum)
        {
            string str = string.Empty;
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "declare @tblAvgKWh as table(energyused numeric(8, 2)); " +
                                  "insert into @tblAvgKWh " +
                                  "select top 12 CAST(energyused as numeric(8,2)) " +
                                  "from arstrxhdr " +
                                  "where consumerid = @consumerid and trxid='EB' " +
                                  "order by trxdate desc; " +
                                  "select AVG(energyused) averageKWh from @tblAvgKWh; ";

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
                            str = rdr["averageKWh"].ToString();
                        }
                        else str = (0).ToString();
                    }
                }
                catch (Exception ex)
                {
                    str = (0).ToString();
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }
            return str;
        }

        private string getClaimedDate(string acctnum)
        {
            string str = string.Empty;
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select ISNULL(CAST(dateclaimburial as varchar(10)),'') [claimeddate] from arsconsumer where consumerid=@consumerid ";

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
                            str = rdr["claimeddate"].ToString();
                        }

                    }
                }
                catch (Exception ex)
                {
                    str = string.Empty;
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }
            }
            return str;
        }

        //added by CJ 09/13/2022
        private bool insertClaimantDetails(BurialClaimantModel bcm)
        {
            bool result = true;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "insert into tbl_burialclaimants(burialresulthdrid,mcdateofdeath,mccauseofdeath,claimantname,claimantaddress,relationship,contactnum) " +
                                  "values(@burialresultheaderid,@mcdateofdeath,@mccauseofdeath,@claimantname,@claimantaddress,@relationship,@contactnum)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@burialresultheaderid", bcm.BurialHeaderId);
                cmd.Parameters.AddWithValue("@mcdateofdeath", bcm.MCDateOfDeath);
                cmd.Parameters.AddWithValue("@mccauseofdeath", bcm.MCCauseOfDeath);
                cmd.Parameters.AddWithValue("@claimantname", bcm.ClaimantName);
                cmd.Parameters.AddWithValue("@claimantaddress", bcm.ClaimantAddress);
                cmd.Parameters.AddWithValue("@relationship", bcm.Relationship);
                cmd.Parameters.AddWithValue("@contactnum", bcm.ContactNum);

                try
                {
                    cmd.ExecuteNonQuery();
                    result = true;
                }catch(Exception ex)
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

        private DataTable getClaimantData(int hdrid)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select mcdateofdeath[DateOfDeath],mccauseofdeath[CauseOfDeath],claimantname[ClaimantName],claimantaddress[ClaimantAddress],relationship[RelToClaimant],contactnum[ContactNo], " +
                                  "'ROSARIO E. CALALANG'[ScreenedBy],'MSD Chief'[ScreenedByPos],'ENGR. DANNY L. MALONZO'[RecommendedBy],'OIC - ISD'[RecommendedByPos],'ALLAN G. BERMUDEZ'[ApprovedBy],'General Manager'[ApprovedByPos] " +
                                  "from tbl_burialclaimants " +
                                  "where burialresulthdrid = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    dt = null;
                }
                finally
                {
                    da.Dispose();
                    cmd.Dispose();
                    con.Close();
                }
            }

            return dt;
        }
    }
}