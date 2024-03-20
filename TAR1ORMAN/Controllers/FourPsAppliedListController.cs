using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace TAR1ORMAN.Controllers
{
    public class FourPsAppliedListController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,MSERVE")]
        // GET: FourPsAppliedList
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAppliedList()
        {
            var jsonResult = Json(new { data = getAllAppliedFourPs() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult PreviewQualifiedLifelinerReport(int rptid)
        {
            //initialize parameters
            DataTable dtData = getDataForReportForm(rptid);

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


        private List<FourPsModel> getAllAppliedFourPs()
        {
            List<FourPsModel> lstafpm = new List<FourPsModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,dateapplied,isQualified,app_lname,app_fname,app_mname," +
                                               "(case when app_addhouseno IS NOT NULL then app_addhouseno + ', ' else '' end) + " +
                                               "(case when app_addstreet IS NOT NULL then app_addstreet + ', ' else '' end) + " +
                                               "(case when app_addbarangay IS NOT NULL then app_addbarangay + ', ' else '' end) + " +
                                               "(case when app_addmunicipality IS NOT NULL then app_addmunicipality + ', ' else '' end) + " +
                                               "(case when app_addprovince IS NOT NULL then app_addprovince else '' end) [address] " +
                                               "from [dbo].[tbl_qualifiedFPQME] order by dateapplied desc; ";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstafpm.Add(new FourPsModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                DateApplied = Convert.ToDateTime(dr["dateapplied"]).ToString("MM-dd-yyyy"),
                                IsQualified = Convert.ToBoolean(dr["isQualified"]),
                                Surname = dr["app_lname"].ToString(),
                                Givenname = dr["app_fname"].ToString(),
                                Middlename = dr["app_mname"].ToString(),
                                Address = dr["address"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstafpm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstafpm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstafpm;
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
    }
}