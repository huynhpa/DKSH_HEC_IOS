using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace DKSHApplication.Common
{
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext context = HttpContext.Current;

                // check if session supported
                if (context.Session != null)
                {
                    if (context.Session[Constant.USER_SESSION] == null)
                    {
                        FormsAuthentication.SignOut();
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "Controller", "Login" },
                            { "Action", "Index" }
                        });
                        return;
                    }
                }
                base.OnActionExecuting(filterContext);
            }
    }
}