using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Configuration;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.DBService;
using TAR1ORDATA.Filters;
using System.Web.Security;

namespace TAR1ORMAN.Controllers
{
    public class SettingsController : Controller
    {
        IDBService idbservice;

        [Authorize(Roles = "IT,SYSADMIN")]
        // GET: Settings
        public ActionResult DBSettings()
        {
            DBSettingModel dbsm = new DBSettingModel();

            Configuration config = null;

            config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

            string currConnectionString = config.ConnectionStrings.ConnectionStrings["getconnstr"].ConnectionString;

            string[] splconstr = currConnectionString.Split(';');

            string servername = splconstr[0].Substring(splconstr[0].IndexOf("=") + 1);
            string dbname = splconstr[1].Substring(splconstr[1].IndexOf("=") + 1);
            string userid = splconstr[2].Substring(splconstr[2].IndexOf("=") + 1);
            string pass = splconstr[3].Substring(splconstr[3].IndexOf("=") + 1);

            dbsm.ServerName = servername;
            dbsm.DatabaseName = dbname;
            dbsm.UserID = userid;
            dbsm.Password = pass;

            return View(dbsm);
        }

        [Authorize(Roles = "IT,SYSADMIN")]
        [HttpPost]
        public ActionResult DBSettings(DBSettingModel dbsm)
        {
            if (ModelState.IsValid)
            {
                string newConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Integrated Security=False;Encrypt=False;Packet Size=4096;",
                                                           dbsm.ServerName, dbsm.DatabaseName, dbsm.UserID, dbsm.Password);

                idbservice = new DBService();

                if(idbservice.IsValidConnectionString(newConnectionString))
                {
                    Configuration config = null;

                    config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    config.ConnectionStrings.ConnectionStrings["getconnstr"].ConnectionString = newConnectionString;
                    config.ConnectionStrings.ConnectionStrings["getconnstr"].ProviderName = "System.Data.SqlClient";
                    config.Save(ConfigurationSaveMode.Modified);

                    FormsAuthentication.SignOut();
                    Session["isAuthAdmin"] = false;
                    Session.Clear();
                    return RedirectToAction("Login", "Authentication");

                }
                else
                {
                    ModelState.AddModelError("ConnectionError", "Specified Connection Settings Is Not Valid.");
                    return View();
                }

            }
            else
            {
                return View();
            }

            
        }
    }
}