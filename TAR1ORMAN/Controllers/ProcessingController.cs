using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.AuditTrailService;
using TAR1ORDATA.DataService.HomeService;
using TAR1ORDATA.Filters;

namespace TAR1ORMAN.Controllers
{
    
    public class ProcessingController : Controller
    {
        IHomeService ihomeservice;
        // GET: Processing
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult ORRangeInAddition()
        {
            return View();
        }

        public JsonResult PreviewORRangeInAddition(TransactionSearchModel tsm)
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string datenow = DateTime.Now.ToShortDateString();
            string errmsg = string.Empty;

            ihomeservice = new HomeService();

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 1; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Previewed data of ORNumber from " + tsm.ORNumberFrom + " to " + tsm.ORNumberTo + " using added value of: " + tsm.Addend + ".";
            atm.TableAffected = "arspaytrxhdr, arspaytrxdtl, arspaytrxapply";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            return Json(ihomeservice.ListOfORInAddition(tsm), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcessORRangeInAddition(TransactionSearchModel tsm)
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string datenow = DateTime.Now.ToShortDateString();
            string errmsg = string.Empty;

            ihomeservice = new HomeService();

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 3; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Processed data of ORNumber from " + tsm.ORNumberFrom + " to " + tsm.ORNumberTo + " using added value of: " + tsm.Addend + ".";
            atm.TableAffected = "arspaytrxhdr, arspaytrxdtl, arspaytrxapply";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            //return Json(ihomeservice.SetNewORByRangeAdd(tsm.ORNumberFrom,tsm.ORNumberTo,tsm.Addend), JsonRequestBehavior.AllowGet);
            return Json(ihomeservice.ProcessNewORByRangeAdd(tsm.ORNumberFrom, tsm.ORNumberTo, tsm.Addend), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult ORRangeInSubtraction()
        {
            return View();
        }

        public JsonResult PreviewORRangeInSubtraction(TransactionSearchModel tsm)
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string datenow = DateTime.Now.ToShortDateString();
            string errmsg = string.Empty;

            ihomeservice = new HomeService();

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 1; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Previewed data of ORNumber from " + tsm.ORNumberFrom + " to " + tsm.ORNumberTo + " using subracted value of: " + tsm.Subtrahend + ".";
            atm.TableAffected = "arspaytrxhdr, arspaytrxdtl, arspaytrxapply";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            return Json(ihomeservice.ListOfORInSubtraction(tsm), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcessORRangeInSubtraction(TransactionSearchModel tsm)
        {
            IAuditTrailService iats;
            iats = new AuditTrailService();

            string datenow = DateTime.Now.ToShortDateString();
            string errmsg = string.Empty;

            ihomeservice = new HomeService();

            //saving to audit trail
            AuditTrailModel atm = new AuditTrailModel();
            atm.Id = 0;
            atm.MadeById = User.Identity.Name;
            atm.ProcessTypeId = 3; //1=Selection of data;2=Insertion of data;3=Modification of data;4=Deletion of data.
            atm.ProcessMade = "Process data of ORNumber from " + tsm.ORNumberFrom + " to " + tsm.ORNumberTo + " using subtracted value of: " + tsm.Subtrahend + ".";
            atm.TableAffected = "arspaytrxhdr, arspaytrxdtl, arspaytrxapply";
            atm.MadeDateTime = datenow;

            if (iats.LogtoAuditTrail(atm))
                errmsg = "Success";
            else
                errmsg = "Unable to log this process.";

            //return Json(ihomeservice.SetNewORByRangeDiff(tsm.ORNumberFrom, tsm.ORNumberTo, tsm.Subtrahend), 
            //    JsonRequestBehavior.AllowGet);
            return Json(ihomeservice.ProcessNewORByRangeDiff(tsm.ORNumberFrom, tsm.ORNumberTo, tsm.Subtrahend),
                JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "AREAMNGR,AUDIT,BILLING,FINHEAD,IT,MDTO,MREADING,MSERVE,SYSADMIN,TELLER,TEMPO,TRAINEE,TREMOTE")]
        public ActionResult PrefixORRange()
        {
            return View();
        }

        public JsonResult PreviewNewPrefixORRange(TransactionSearchModel tsm)
        {
            ihomeservice = new HomeService();
            return Json(ihomeservice.ListNewPrefixOfORRange(tsm), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcessNewPrefixORRange(TransactionSearchModel tsm)
        {
            ihomeservice = new HomeService();
            return Json(ihomeservice.ProcessNewPrefixOfORRange(tsm.ORNumberFrom,tsm.ORNumberTo,
                tsm.NewPrefix,tsm.Subtrahend,tsm.Addend) , JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsNewOrRangeVacant(string orfr, string orto, int adddiffval, bool isaddition)
        {
            ihomeservice = new HomeService();
            return Json(ihomeservice.isNewORVacant(orfr, orto, adddiffval, isaddition), JsonRequestBehavior.AllowGet);
        }
       
    }
}