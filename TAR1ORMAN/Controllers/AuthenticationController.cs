using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService;
using TAR1ORDATA.DataService.UserLoginService;
using TAR1ORDATA.Globals;

namespace TAR1ORMAN.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserLoginService userlservice;
        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel ulm, string returnUrl)
        {

            userlservice = new UserLoginService();

            UserLoginModel logulm = new UserLoginModel();

            if (ModelState.IsValid)
            {
                if (userlservice.GetUserByIdAndPass(ulm.UserId, ulm.Password))
                {
                    FormsAuthentication.SetAuthCookie(ulm.UserId, false);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("CredentialError", "Invalid user id or password.");
                    return View("Login");
                }
                
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            System.Web.HttpContext.Current.Session.RemoveAll();

            ViewBag.Title = "Login";

            return RedirectToAction("Login", "Authentication");
        }
    }
}