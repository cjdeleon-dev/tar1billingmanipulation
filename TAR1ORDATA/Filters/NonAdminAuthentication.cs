using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAR1ORDATA.Filters
{
    public class NonAdminAuthentication: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Convert.ToBoolean(filterContext.HttpContext.Session["isAuthNonAdmin"]))
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };
            }
        }
    }
}