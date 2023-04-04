using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.TopActiveDisconService;

namespace TAR1ORMAN.Controllers
{
    public class TopActiveDisconByTownController : Controller
    {
        ITopActiveDisconService itads;
        private static GridView gv = new GridView();

        [Authorize(Roles = "AUDIT,FINHEAD,IT,SYSADMIN,AREAMNGR")]
        // GET: TopActiveDisconByTown
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStatus()
        {
            itads = new TopActiveDisconService();
            return Json(itads.GetAllStatus(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTown()
        {
            itads = new TopActiveDisconService();
            return Json(itads.GetAllTown(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult loadfordata(string stattowntop)
        {
            string[] _stattowntop = stattowntop.Split('_');
            string stat = _stattowntop[0];
            string town = _stattowntop[1];
            string top = _stattowntop[2];

            itads = new TopActiveDisconService();
            List<HighArrearsConsumerModel> data = itads.GetTopHundred(stat, town, top);
            if (gv.DataSource != null)
                gv.DataSource = null;
            gv.DataSource = data;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string datenow = DateTime.Now.ToShortDateString();
            Response.AddHeader("content-disposition", "attachment; filename=Top100High_" + datenow + ".xls");
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