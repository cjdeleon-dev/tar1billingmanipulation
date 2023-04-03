using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.AuditTrailService;
using TAR1ORDATA.DataService.DTDWithCurrBillService;

namespace TAR1ORMAN.Controllers
{
    public class DTDWithCurrentBillsController : Controller
    {

        IDTDWithCurrBillService idtdwcb;
        // GET: DTDWithCurrentBills
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult loadfordata()
        {
            idtdwcb = new DTDWithCurrBillService();
            var data = idtdwcb.GetAllDTDWithCurrBills();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string errmsg = string.Empty;

            idtdwcb = new DTDWithCurrBillService();
            var gv = new GridView();
            gv.DataSource = idtdwcb.GetAllDTDWithCurrBills();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;

            string datenow = DateTime.Now.ToShortDateString();

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 1; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Generated and exported data for Disconnected Consumers with current bill."; ;
            atm.TableAffected = "arstrxhdr, arstrxdtl";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            Response.AddHeader("content-disposition", "attachment; filename=DTDWithCurrBills_" + datenow + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        }
    }
}