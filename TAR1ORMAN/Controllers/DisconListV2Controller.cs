using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataModel;

namespace TAR1ORMAN.Controllers
{
    public class DisconListV2Controller : Controller
    {
        
        private static List<DisconListModel> disconlist = new List<DisconListModel>();

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MSERVE,SYSADMIN,TELLER")]
        // GET: DisconListV2
        [HttpGet]
        public ActionResult Index()
        {
            return View();
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
        public JsonResult GenerateDisconList(string ridnomostatid)
        {
            List<DisconListModel> data = new List<DisconListModel>();

            string[] _ridnomostatid = ridnomostatid.Split('_');
            string routeid = _ridnomostatid[0];
            //string bperiod = _ridbpoidnomostatid[1];
            //string officeid = _ridbpoidnomostatid[2];
            string numofmo = _ridnomostatid[1];
            string disconstatid = _ridnomostatid[2];

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                conn.Open();

                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandText = "sp_GenerateSubjForDisconV2";
                da.SelectCommand.Parameters.AddWithValue("@filternum",Convert.ToInt32(numofmo));
                da.SelectCommand.Parameters.AddWithValue("@status", Convert.ToInt32(disconstatid));
                da.SelectCommand.Parameters.AddWithValue("@route", routeid);

                DataTable dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            data.Add(new DisconListModel {
                                AccountNo = dr["AccountNo"].ToString(),
                                AccountName = dr["Name"].ToString(),
                                Address = dr["Address"].ToString(),
                                MeterNo = dr["MeterNo"].ToString(),
                                FirstBill = dr["FromBP"].ToString(),
                                LastBill = dr["ToBP"].ToString(),
                                NoOfMonths = Convert.ToInt32(dr["NumberOfMonths"]),
                                Due = Convert.ToDouble(dr["TotalAmountDue"]),
                                Remark = dr["Remarks"].ToString()
                            });
                        }
                    }
                }
                catch(Exception ex)
                {
                    data = null;
                }
                finally
                {
                    conn.Close();
                }

            }

            //if (gv.DataSource != null)
            //    gv.DataSource = null;
            //gv.DataSource = data;

            if (disconlist != null)
                disconlist = null;
            disconlist = data;

            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SaveDisconListHeader(string ridbpoidnomostatid)
        {
            bool isSuccess = false;

            string[] _ridbpoidnomostatid = ridbpoidnomostatid.Split('_');
            string routeid = _ridbpoidnomostatid[0];
            string bperiod = _ridbpoidnomostatid[1];
            string officeid = _ridbpoidnomostatid[2];
            string numofmo = _ridbpoidnomostatid[3];
            string disconstatid = _ridbpoidnomostatid[4];

            string userid = User.Identity.Name;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into tbl_disconlisthdr([routeid],[billperiod],[officeid],[numofmonths],[disconstatusid],[generateddatetime],[generatedby]) " +
                                  "values(@routeid,@billperiod,@officeid,@numofmonths,@disconstatusid,getdate(),@userid);";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@routeid", routeid);
                cmd.Parameters.AddWithValue("@billperiod", bperiod);
                cmd.Parameters.AddWithValue("@officeid", officeid);
                cmd.Parameters.AddWithValue("@numofmonths", numofmo);
                cmd.Parameters.AddWithValue("@disconstatusid", disconstatid);
                cmd.Parameters.AddWithValue("@userid", userid);

                SqlTransaction trans;
                trans = conn.BeginTransaction();

                cmd.Transaction = trans;

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    isSuccess = false;
                }
                finally
                {
                    trans.Dispose();
                    cmd.Dispose();
                }

            }

            if (isSuccess)
                isSuccess = isSaveDisconListDetails();

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        private bool isSaveDisconListDetails()
        {
            int headerid = getDisconListHeaderIdByUserId();

            if (disconlist.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
                {
                    conn.Open();

                    foreach (DisconListModel dlm in disconlist)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into tbl_disconlistdtl(disconlisthdrid,consumerid,firstbillperiod,lastbillperiod,noofmonths,amount,remark,isdiscon) " +
                                          "values(@hdrid,@consumerid,@fbp,@lbp,@noofmo,@amount,@remark,0);";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@hdrid", headerid);
                        cmd.Parameters.AddWithValue("@consumerid", dlm.AccountNo);
                        cmd.Parameters.AddWithValue("@fbp", dlm.FirstBill);
                        cmd.Parameters.AddWithValue("@lbp", dlm.LastBill);
                        cmd.Parameters.AddWithValue("@noofmo", dlm.NoOfMonths);
                        cmd.Parameters.AddWithValue("@amount", dlm.Due);
                        cmd.Parameters.AddWithValue("@remark", dlm.Remark);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            //clear disconlist
                            disconlist = null;
                            return false;
                        }
                        finally
                        {
                            cmd.Dispose();
                        }
                    }

                }
            }

            //clear disconlist
            disconlist = null;
            return true; 
        }

        [HttpPost]
        public JsonResult SaveDisconListCrews(List<DisconCrewModel> lstdisconcrews)
        {
            int headerid = getDisconListHeaderIdByUserId();
            if (lstdisconcrews.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString()))
                {
                    conn.Open();

                    foreach (DisconCrewModel dcm in lstdisconcrews)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into tbl_disconlistcrews(disconlisthdrid,disconcrewid) " +
                                          "values(@hdrid,@crewid);";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@hdrid", headerid);
                        cmd.Parameters.AddWithValue("@crewid", dcm.Id);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return Json(new { savemsg = ex.Message }, JsonRequestBehavior.AllowGet);
                        }
                        finally
                        {
                            cmd.Dispose();
                        }
                    }

                }
                return Json(new { savemsg = "Successfully Saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { savemsg = "No Crew List to be saved." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult ExportData()
        {
            int hdrid = getDisconListHeaderIdByUserId();
            try
            {
                GridView gv = new GridView();
                gv.DataSource = getDataToExport(hdrid);
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment; filename=ForDisconList_Id_" + hdrid + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                throw;
            }

            return View();
        }


        public int getDisconListHeaderIdByUserId()
        {
            int resval = 0;

            string userid = User.Identity.Name;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                cmd.Connection.Open();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select max(id)[disconlisthdrid] from tbl_disconlisthdr where generatedby=@userid;";

                cmd.Parameters.AddWithValue("@userid", userid);

                SqlDataReader rdr;

                try
                {
                    rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            resval = Convert.ToInt32(rdr["disconlisthdrid"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    resval = 0;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }

            return resval;
        }

        public List<DisconListModel> getDataToExport(int hdrid)
        {
            List<DisconListModel> lstdlm = new List<DisconListModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["getconnstr"].ToString());
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select dtl.consumerid, cons.name, cons.address,cons.mtrserialno,dtl.firstbillperiod,dtl.lastbillperiod,dtl.noofmonths,dtl.amount,dtl.remark " +
                                               "from tbl_disconlistdtl dtl inner join arsconsumer cons on dtl.consumerid = cons.consumerid " +
                                               "where disconlisthdrid = @hdrid;";

                da.SelectCommand.Parameters.AddWithValue("@hdrid", hdrid);

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstdlm.Add(new DisconListModel
                            {
                               AccountNo = dr["consumerid"].ToString(),
                               AccountName = dr["name"].ToString(),
                               Address = dr["address"].ToString(),
                               MeterNo = dr["mtrserialno"].ToString(),
                               FirstBill = dr["firstbillperiod"].ToString(),
                               LastBill = dr["lastbillperiod"].ToString(),
                               NoOfMonths = Convert.ToInt32(dr["noofmonths"]),
                               Due= Convert.ToDouble(dr["amount"]),
                               Remark = dr["remark"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstdlm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstdlm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }
            return lstdlm;
        }
    }
}