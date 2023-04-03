using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataService.EmpPowerBillService;

namespace TAR1ORMAN.Controllers
{
    public class EmpPowerBillController : Controller
    {
        IEmpPowerBillService iepbs;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        // GET: EmpPowerBill
        public ActionResult EmpCareOff()
        {
            if (User.IsInRole("SYSADMIN"))
            {
                ViewBag.Message = "ADMIN";
            }
            else
            {
                ViewBag.Message = "NONADMIN";
            }

            return View();
        }

        [HttpGet]
        public ActionResult loadfordata()
        {
            iepbs = new EmpPowerBillService();
            var data = iepbs.GetAllEmpPowerBills();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {
            iepbs = new EmpPowerBillService();
            var gv = new GridView();
            gv.DataSource = iepbs.GetAllEmpPowerBills();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();
            Response.AddHeader("content-disposition", "attachment; filename=EmpPowerBill_" + datenow + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("EmpCareOff");
        }
    }
}