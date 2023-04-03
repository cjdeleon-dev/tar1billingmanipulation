using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataService.SeniorCitizenService;

namespace TAR1ORMAN.Controllers
{
    public class SeniorCitizenController : Controller
    {
        ISeniorCitizenService iscs;
        // GET: SeniorCitizen
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
            iscs = new SeniorCitizenService();
            var data = iscs.GetAllSeniorCitizens();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}