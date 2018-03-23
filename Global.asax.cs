using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using static RealState.Security.RealstatePrincipal;
using RealState.Security;
using Newtonsoft.Json;

namespace RealState
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie AuthCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (AuthCookie != null)
            {
                FormsAuthenticationTicket AuthTicket = FormsAuthentication.Decrypt(AuthCookie.Value);
                PrincipalSerializeModel SerializaModel = JsonConvert.DeserializeObject<PrincipalSerializeModel>(AuthTicket.UserData);
                RealstatePrincipal NewUser = new RealstatePrincipal(AuthTicket.Name);
                NewUser.FirstName = SerializaModel.FirstName;
                NewUser.LastName = SerializaModel.LastName;
                NewUser.UserID = SerializaModel.UserID;
          
                NewUser.Roles = SerializaModel.Roles;
                HttpContext.Current.User = NewUser;
            }

        }
    }
}
