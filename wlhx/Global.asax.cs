using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using wlhx.Controllers;

namespace wlhx
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        SessionController sc = new SessionController();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string[] url = app.Request.Url.ToString().Split('/');
            if (app.Request.Browser.MajorVersion <= 8 && (app.Request.Browser.Browser == "InternetExplorer" || app.Request.Browser.Browser == "IE"))
            {
                if (!(url.Length >= 5 && url[4] == "BrowserError"))
                {
                    Context.Response.Redirect("/Home/BrowserError");
                }
            }
            UserController.sc = sc;
            StudentController.sc = sc;
            if (!(url[3] == "Home" || url[3]=="Video" || url[3] == "" || url[4] == "Login" || url[4] == "LoginIn" || url[4] == "VerCode" || url[4] == "requestData"))
            {
                if (url[3] == "User" && sc.GetUserSession() == null)
                {
                    Context.Response.Redirect("/User/Login");
                }
                if (url[3] == "Student" && sc.GetStudentSession() == null)
                {
                    Context.Response.Redirect("/Home/Index");
                }
                //else if (url[3] == "Student" && sc.GetStudentSession() == null)
                //{
                //    Context.Response.Redirect("/Student/Login");
                //}
            }
        }
    }
}