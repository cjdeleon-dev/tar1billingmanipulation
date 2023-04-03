using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAR1ORDATA.Filters
{
    public class AdminAuthentication: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Convert.ToBoolean(filterContext.HttpContext.Session["isAuthAdmin"]))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };
            }
        }
    }
}