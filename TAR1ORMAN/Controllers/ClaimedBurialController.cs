using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.ClaimedBurialService;
using PagedList;
using TAR1ORDATA.Filters;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace TAR1ORMAN.Controllers
{
    public class ClaimedBurialController : Controller
    {
        IClaimedBurialService icbs;
        // GET: ClaimedBurial
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult Index()
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
            icbs = new ClaimedBurialService();
            var data = icbs.GetAllClaimedBurialConsumers();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        
    }
}
