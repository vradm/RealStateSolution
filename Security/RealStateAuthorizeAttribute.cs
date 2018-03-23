using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
namespace RealState.Security
{
    public class RealStateAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual RealstatePrincipal CurrentUser
        {
            get { return HttpContext.Current.User as RealstatePrincipal; }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new  {Controller= "Error", action = "Unauthorized", area = ""}));
                }
            }
            if (!String.IsNullOrEmpty(Users))
            {
                if (!Users.Contains(CurrentUser.UserID.ToString()))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AuthenticationFails", area = "" }));
                    // base.OnAuthorization(filterContext); //returns to login url
                }
            }
        }
            else
            {
                string returnUrl = null;
                if (filterContext.HttpContext.Request.HttpMethod.Equals("GET", System.StringComparison.CurrentCultureIgnoreCase))
                    returnUrl = filterContext.HttpContext.Request.RawUrl;
                base.OnAuthorization(filterContext); //returns to login url
    }
}
    }
}
        
   