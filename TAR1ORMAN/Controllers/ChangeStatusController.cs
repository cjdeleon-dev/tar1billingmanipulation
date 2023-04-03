using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.ChangeStatusService;


namespace TAR1ORMAN.Controllers
{
    public class ChangeStatusController : Controller
    {
        IChangeStatusService icss;

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MSERVE,SYSADMIN,TELLER")]
        // GET: ChangeStatus
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult loadfordata(int status=1)
        {
            icss = new ChangeStatusService();
            List<ConsumerModel> data;

            if (status==1)
                data = icss.GetAllActiveConsumers();
            else
                data = icss.GetAllDisconConsumers();

            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SetStatusOfConsumerById(ConsumerStatusLogModel cslm)
        {
            icss = new ChangeStatusService();

            string userid = User.Identity.Name;
            cslm.EntryUser = userid;

            if (icss.SetStatusOfConsumerId(cslm))
                return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Fail" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadStatusLogData(string accountno)
        {
            icss = new ChangeStatusService();

            List<ConsumerStatusLogModel> data;

            data = icss.GetStatusLogByConsumerid(accountno);

            var jsonResult = Json(new { data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetAllStatus(string exceptstat)
        {
            icss = new ChangeStatusService();
            return Json(icss.GetAllStatus(exceptstat), JsonRequestBehavior.AllowGet);

        }
    }
}