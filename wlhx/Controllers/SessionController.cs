using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wlhx.Controllers
{
    public class SessionController : Controller
    {
        //
        // GET: /Session/

        public object GetUserSession()
        {
            return System.Web.HttpContext.Current.Session["user"];
        }

        public void SetUserSession(object value)
        {
            System.Web.HttpContext.Current.Session["user"] = value;
        }

        public object GetStudentSession()
        {
            return System.Web.HttpContext.Current.Session["student"];
        }

        public void SetStudentSession(object value)
        {
            System.Web.HttpContext.Current.Session["student"] = value;
        }

    }
}
