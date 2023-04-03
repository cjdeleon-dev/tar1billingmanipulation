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
    public class DisconRptSummaryListController : Controller
    {
        // GET: DisconRptSummaryList
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult loadfordata()
        {
            var jsonResult = Json(new { data = getAllSumDisconRpts() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }


        public ActionResult PrintGeneratedSummaryOfDisconReport(int headerId)
        {
           
            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptSummaryOfDiscon.rdlc");
            lr.ReportPath = p;

            //initialize parameters

            DataTable dtHeaderData = getHeaderReport(headerId);
            DataTable dtDetailsData = getDetailReport(headerId);
            DataTable dtCrewsData = getCrewsReport(headerId);
            DataTable dtSummaryData = getTypeSummary(headerId);

            //ReportDataSource for Header
            ReportDataSource sodrhdr = new ReportDataSource("dsSummaryOfDisconRptHdr", dtHeaderData);
            ReportDataSource sodrdtl = new ReportDataSource("dsSummaryOfDisconRptDtl", dtDetailsData);
            ReportDataSource sodrcrw = new ReportDataSource("dsSummaryOfDisconRptCrews", dtCrewsData);
            ReportDataSource sodrsum = new ReportDataSource("dsTypeSummary", dtSummaryData);

            lr.DataSources.Add(sodrhdr);//Header
            lr.DataSources.Add(sodrdtl);//Details
            lr.DataSources.Add(sodrcrw);//Crews
            lr.DataSources.Add(sodrsum);//Summary

            string mt, enc, f;
            string[] s;
            Warning[] w;

            //Rendering
            byte[] b = lr.Render("PDF", null, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }



        private List<SumDTDRptHeaderModel> getAllSumDisconRpts()
        {
            List<SumDTDRptHeaderModel> lstsdrhm = new List<SumDTDRptHeaderModel>();

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    try
                    {
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.Connection = con;
                        da.SelectCommand.CommandType = CommandType.Text;
                        da.SelectCommand.CommandText = "select hdr.id[Id],gendatetime[DateTimeGenerated],UPPER(RTRIM(genusr.name))[GeneratedBy]," +
                                                       "UPPER(RTRIM(chk.name))[CheckedBy],UPPER(ofc.manager)[NotedBy],Routeid " +
                                                       "from tbl_sumdtdrpthdr hdr " +
                                                       "inner join secuser genusr " +
                                                       "on hdr.genuserid = genusr.userid " +
                                                       "inner join secuser chk " +
                                                       "on hdr.checkbyid = chk.userid " +
                                                       "inner join tbloffices ofc " +
                                                       "on hdr.officeid=ofc.id;";

                    
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach(DataRow dr in dt.Rows)
                            {
                                lstsdrhm.Add(new SumDTDRptHeaderModel
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    DateTimeGenerate = dr["DateTimeGenerated"].ToString(),
                                    GenerateUserId = dr["GeneratedBy"].ToString(),
                                    CheckBy = dr["CheckedBy"].ToString(),
                                    NotedBy = dr["NotedBy"].ToString(),
                                    RouteId = dr["RouteId"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lstsdrhm = null;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return lstsdrhm;
        }

        private DataTable getHeaderReport(int hdrid)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select hdr.id[Id],gendatetime[DateTimeGenerated],UPPER(RTRIM(genusr.name))[GeneratedBy],UPPER(RTRIM(chk.name))[CheckedBy],UPPER(ofc.manager)[NotedBy],Routeid " +
                                  "from tbl_sumdtdrpthdr hdr " +
                                  "inner join secuser genusr " +
                                  "on hdr.genuserid = genusr.userid " +
                                  "inner join secuser chk " +
                                  "on hdr.checkbyid = chk.userid " +
                                  "inner join tbloffices ofc " +
                                  "on hdr.officeid=ofc.id " +
                                  "where hdr.id = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

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

        private DataTable getCrewsReport(int hdrid)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select crw.Id, sumdtdrpthdrid[SummaryOfDisconHeaderId],disconcrewid[DisconCrewId],dcw.crewname[DisconCrew] " +
                                  "from tbl_sumdtdrptcrews crw " +
                                  "inner join tbl_disconcrews dcw " +
                                  "on crw.disconcrewid = dcw.id " +
                                  "where sumdtdrpthdrid = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

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

        private DataTable getDetailReport(int hdrid)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select dtl.Id, sumdtdrpthdrid[SummaryOfDisconHeaderId],ROW_NUMBER() OVER(ORDER BY dtl.Id)[No],dtl.consumerid[ConsumerId]," +
                                  "rtrim(cons.name)[ConsumerName],cons.consumertypeid[ConsumerType],dtl.reason[Reason],dtl.lastreading[LastReading], " +
                                  "enc.initials[Encoder],hdr.actiondate[ActualDate] " +
                                  "from tbl_sumdtdrptdtl dtl " +
                                  "inner join arsconsumer cons " +
                                  "on dtl.consumerid = cons.consumerid " +
                                  "left join secuser enc " +
                                  "on dtl.encodeuserid = enc.userid " +
                                  "inner join tbl_sumdtdrpthdr hdr " +
                                  "on dtl.sumdtdrpthdrid = hdr.id " +
                                  "where sumdtdrpthdrid = @hdrid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

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

        private DataTable getTypeSummary(int hdrid)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select UPPER(case when c.description NOT IN('Residential', 'Commercial', 'Industrial') " +
                                  "then 'OTHERS' else c.description end)[Type],count(c.description)[Disconnected], " +
                                  "case when rtrim(c.description)= 'Residential' then " +
                                  "CAST(count(c.description) * 50 AS numeric(18, 2)) " +
                                  "else case when rtrim(c.description)= 'Commercial' then " +
                                  "CAST(count(c.description) * 100 AS numeric(18, 2)) " +
                                  "else case when rtrim(c.description)= 'Industrial' then " +
                                  "CAST(count(c.description) * 500 AS numeric(18, 2)) " +
                                  "else	CAST(0 as numeric(18, 2)) " +
                                  "end end end[Commission] " +
                                  "from tbl_sumdtdrptdtl a " +
                                  "inner join arsconsumer b " +
                                  "on a.consumerid = b.consumerid " +
                                  "inner join arstype c " +
                                  "on b.consumertypeid = c.consumertypeid " +
                                  "where sumdtdrpthdrid =  @hdrid " +
                                  "group by c.description; ";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@hdrid", hdrid);

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