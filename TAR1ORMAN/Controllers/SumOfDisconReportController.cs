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
    public class SumOfDisconReportController : Controller
    {
        [Authorize(Roles = "AREAMNGR,IT,MDTO,SYSADMIN,FINHEAD,BILLING")]
        // GET: SumOfDisconReport
        public ActionResult SumDisconRpt()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCurrentBillPeriod()
        {
            BillPeriodModel bpm = new BillPeriodModel();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                cmd.Connection.Open();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select top 1 rtrim(billperiod)[billperiod],rtrim(description)[description] from arsbillperiod order by billperiod desc;";

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            bpm.Id = Convert.ToInt32(rdr["billperiod"]);
                            bpm.BillPeriod = rdr["description"].ToString();
                        }
                    }
                    else
                        bpm = null;
                }
                catch (Exception ex)
                {
                    bpm = null;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            return Json(bpm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllOffices()
        {
            List<OfficeModel> lstrm = new List<OfficeModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select [id], [displaytext] from tbloffices;";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstrm.Add(new OfficeModel
                            {
                                Id = Convert.ToInt32(dr["id"].ToString()),
                                DisplayText = dr["displaytext"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstrm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstrm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstrm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllRoutesByOfficeId(int id)
        {
            List<RouteModel> lstrm = new List<RouteModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select [routeid], [description] from arsroute where LEFT(routeid,2) in (select areacode from tblofficeareas where officeid=@ofid);";
                da.SelectCommand.Parameters.AddWithValue("@ofid", id);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstrm.Add(new RouteModel
                            {
                                Id = dr["routeid"].ToString(),
                                Route = dr["description"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstrm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstrm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstrm, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllCrewsByOfficeId(int id)
        {
            List<DisconCrewModel> lstdcm = new List<DisconCrewModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select id,crewname from tbl_disconcrews where officeid=@ofid;";
                da.SelectCommand.Parameters.AddWithValue("@ofid", id);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstdcm.Add(new DisconCrewModel
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Name = dr["crewname"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstdcm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstdcm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstdcm, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllDTDConsumerByRouteId(string dtddaterouteid)
        {
            List<DTDConsumerModel> lstdcm = new List<DTDConsumerModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {

                string[] dtdparam = dtddaterouteid.Split('_');
                string dtddate = dtdparam[0];
                string routeid = dtdparam[1];

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;

                da.SelectCommand.CommandText = "select asl.consumerid,cons.name,cons.consumertypeid[type],asl.reason,asl.dtdread,asl.userid,usr.initials[encoder],asl.actdate " +
                                               "from arsstatuslog asl inner join arsconsumer cons " +
                                               "on asl.consumerid = cons.consumerid " +
                                               "left join secuser usr " +
                                               "on asl.userid = usr.userid " +
                                               "where asl.actdate = @actdate and asl.newstatusid = 'D' and asl.consumerid like '__' + @routeid + '%' " +
                                               "and asl.consumerid not in (" +
                                               "    select dtl.consumerid " +
                                               "    from tbl_sumdtdrpthdr hdr " +
                                               "    inner join tbl_sumdtdrptdtl dtl " +
                                               "    on hdr.id = dtl.sumdtdrpthdrid " +
                                               "    where hdr.actiondate = @actdate and dtl.consumerid like '__' + @routeid + '%' " +
                                               ")";

                da.SelectCommand.Parameters.AddWithValue("@actdate", dtddate);
                da.SelectCommand.Parameters.AddWithValue("@routeid", routeid);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstdcm.Add(new DTDConsumerModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                AccountName = dr["name"].ToString(),
                                AccountType = dr["type"].ToString(),
                                Reason = dr["reason"].ToString(),
                                LastReading = dr["dtdread"].ToString(),
                                EncoderId = dr["userid"].ToString(),
                                Encoder = dr["encoder"].ToString(),
                                ActDate = dr["actdate"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstdcm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstdcm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstdcm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFinanceHead()
        {
            List<FinanceHeadModel> lstfhm = new List<FinanceHeadModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {

               
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;

                da.SelectCommand.CommandText = "select userid,UPPER(name)[finhead] from secuser " +
                                               "where workgroupid = 'FINHEAD' " +
                                               "and right(rtrim(name),3)<> 'MRB' " +
                                               "and rtrim(defrevcenterid)<> '' " +
                                               "and userid not in ('0906', '0601', '0521', '0804')";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstfhm.Add(new FinanceHeadModel
                            {
                                UserId = dr["userid"].ToString(),
                                Name = dr["finhead"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstfhm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstfhm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return Json(lstfhm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertNewSumDTDRptHeader(SumDTDRptHeaderModel sdrhm)
        {
            sdrhm.GenerateUserId = User.Identity.Name;
            return Json(insertNewSumDTDRptHeader(sdrhm), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetHeaderIdOfNewInsertedSumDTDRpt()
        {
            string userid = User.Identity.Name;
            return Json(getHeaderIdOfNewInsertedSumDTDRpt(userid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertNewSumDTDRptCrews(List<SumDTDRptCrewModel> lstsdrcm)
        {
            return Json(insertNewSumDTDRptCrews(lstsdrcm), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertNewSumDTDRptDetails(List<SumDTDRptDetailModel> lstsdrdm)
        {
            return Json(insertNewSumDTDRptDetails(lstsdrdm), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintGeneratedSummaryOfDisconReport()
        {
            string userid = User.Identity.Name;
            int headerid = getHeaderIdOfNewInsertedSumDTDRpt(userid);

            LocalReport lr = new LocalReport();
            string p = Path.Combine(Server.MapPath("/Reports"), "rptSummaryOfDiscon.rdlc");
            lr.ReportPath = p;

            //initialize parameters

            DataTable dtHeaderData = getHeaderReport(headerid);
            DataTable dtDetailsData = getDetailReport(headerid);
            DataTable dtCrewsData = getCrewsReport(headerid);
            DataTable dtSummaryData = getTypeSummary(headerid);

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




        //PRIVATE FUNCTIONS AND PROCEDURES
        private bool insertNewSumDTDRptHeader(SumDTDRptHeaderModel sumdtdrpthdr)
        {
            bool result = true;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "insert into tbl_sumdtdrpthdr(gendatetime,genuserid,checkbyid,notedbyid,routeid,officeid,actiondate) " +
                                  "values(getdate(),@genuserid,@checkbyid,@notedbyid,@routeid,@officeid,@actiondate)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@genuserid", sumdtdrpthdr.GenerateUserId);
                cmd.Parameters.AddWithValue("@checkbyid", sumdtdrpthdr.CheckByUserId);
                cmd.Parameters.AddWithValue("@notedbyid", sumdtdrpthdr.NotedByUserId);
                cmd.Parameters.AddWithValue("@routeid", sumdtdrpthdr.RouteId);
                cmd.Parameters.AddWithValue("@officeid", sumdtdrpthdr.OfficeId);
                cmd.Parameters.AddWithValue("@actiondate", sumdtdrpthdr.ActionDate);

                try
                {
                    cmd.ExecuteNonQuery();
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

        private int getHeaderIdOfNewInsertedSumDTDRpt(string generateuserid)
        {
            int id = 0;

            using (SqlCommand cmd = new SqlCommand())
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                con.Open();

                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select max(id) [newid] from tbl_sumdtdrpthdr where genuserid=@genuserid;";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@genuserid", generateuserid);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            id = Convert.ToInt32(rdr["newid"]);
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

        private bool insertNewSumDTDRptCrews(List<SumDTDRptCrewModel> lstsdrcm)
        {
            bool result = true;

            foreach(SumDTDRptCrewModel sdrcm in lstsdrcm)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                    con.Open();

                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "insert into tbl_sumdtdrptcrews(sumdtdrpthdrid,disconcrewid) " +
                                      "values(@sumdtdrpthdrid,@disconcrewid)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@sumdtdrpthdrid", sdrcm.SumDTDRptHeaderId);
                    cmd.Parameters.AddWithValue("@disconcrewid", sdrcm.DisconCrewId);

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

        private bool insertNewSumDTDRptDetails(List<SumDTDRptDetailModel> lstsdrdm)
        {
            bool result = true;

            foreach (SumDTDRptDetailModel sdrdm in lstsdrdm)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                    con.Open();

                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "insert into tbl_sumdtdrptdtl(sumdtdrpthdrid,consumerid,reason,lastreading,encodeuserid) " +
                                      "values(@sumdtdrpthdrid,@consumerid,@reason,@lastreading,@encodeuserid)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@sumdtdrpthdrid", sdrdm.SumDTDRptHeaderId);
                    cmd.Parameters.AddWithValue("@consumerid", sdrdm.ConsumerId);

                    if (sdrdm.Reason == null)
                        cmd.Parameters.AddWithValue("@reason", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@reason", sdrdm.Reason);

                    if(sdrdm.LastReading==null)
                        cmd.Parameters.AddWithValue("@lastreading", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@lastreading", sdrdm.LastReading);
                    if (sdrdm.EncodeUserId == null)
                        cmd.Parameters.AddWithValue("@encodeuserid", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@encodeuserid", sdrdm.EncodeUserId);

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