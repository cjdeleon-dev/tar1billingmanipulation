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
using Microsoft.Reporting.WebForms;
using System.IO;

namespace TAR1ORMAN.Controllers
{
    public class AppliedBurialController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: AppliedBurial
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllAppliedBurial()
        {
            var jsonResult = Json(new { data = getAllAppliedBurial()}, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult PrintBurialApplicationResult(int headerid)
        {
            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptBurialVerificationResult.rdlc");
            lr.ReportPath = p;

            //initialize parameters

            DataTable dtHeaderData = getHeaderResult(headerid);
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



        //PROCEDURES AND FUNCTIONS

        private List<BurialAppValModel> getAllAppliedBurial()
        {
            List<BurialAppValModel> lstbavm = new List<BurialAppValModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select hdr.id,hdr.consumerid,rtrim(cons.name)name,rtrim(address)address,dategen[appvaldate] " +
                                               "from tbl_burialresulthdr hdr " +
                                               "inner join arsconsumer cons " +
                                               "on hdr.consumerid = cons.consumerid;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstbavm.Add(new BurialAppValModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                ConsumerId = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                AppValDate = dr["appvaldate"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstbavm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstbavm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstbavm;
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
                                  "'ROSARIO E. CALALANG'[ScreenedBy],'MSD Chief'[ScreenedByPos],'ENGR. DANNY L. MALONZO'[RecommendedBy],'OIC - ISD'[RecommendedByPos],'ENGR. LORETO A. MARCELINO' [ApprovedBy],'Acting General Manager'[ApprovedByPos] " +
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