using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.Filters;

namespace TAR1ORMAN.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.Name;
            if (User.IsInRole("IT")|| User.IsInRole("SYSADMIN")|| User.IsInRole("FINHEAD")|| User.IsInRole("AREAMNGR")|| User.IsInRole("AUDIT"))
            {
                ViewBag.Message = "ADMIN";
            }
            else
            {
                ViewBag.Message = "NONADMIN";
            }

            return View();
        }
    }
}