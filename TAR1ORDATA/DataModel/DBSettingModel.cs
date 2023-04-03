using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TAR1ORDATA.DataModel
{
    public class DBSettingModel
    {
        [Display(Name = "Server Name:")]
        public string ServerName { get; set; }
        [Display(Name = "Database Name:")]
        public string DatabaseName { get; set; }
        [Display(Name = "User ID:")]
        public string UserID { get; set; }
        [Display(Name = "Password:")]
        public string Password { get; set; }
    }
}