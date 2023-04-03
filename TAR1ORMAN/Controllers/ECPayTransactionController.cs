using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAR1ORMAN.Controllers
{
    public class ECPayTransactionController : Controller
    {
        [Authorize(Roles = "AUDIT,FINHEAD,IT,SYSADMIN")]
        // GET: ECPayTransaction
        public ActionResult ECPay()
        {
            return View();
        }
    }
}