using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wlhx.BLL;
using wlhx.Common;

namespace wlhx.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        public static SessionController sc = null;
        public string Login()
        {
            string Username = Request["Username"];
            string Passwd = Request["Passwd"];
            string yzm = Request["yzm"];
            string json = null;
            long id = 0;
            string yzm_sys = System.Web.HttpContext.Current.Session["ver"].ToString();
            if ((!CheckStudentLogin.Check(Username, Passwd, out id)) || yzm != yzm_sys)
            {
                json = yzm == yzm_sys ? "用户名或密码错误" : "验证码错误";
                return "{\"back\":\"" + json + "\"}";
            }

            sc.SetStudentSession(id);
            return "{\"back\":\"ok\"}";
        }
        public ActionResult Index()
        {
            long id = (long)sc.GetStudentSession();
            ViewBag.student = new StudentOperation().GetStudent(id);
            return View();
        }

        public ActionResult Profession()
        {
            return PartialView();
        }

        public string GetProfessionaLEmphasis()
        {
            return new ProfessionalOperation().GetProfessionaLEmphasis((long)sc.GetStudentSession());
        }

        public JsonResult ProfessionChoose(string id)
        {
            return Json(new StudentOperation().ProfessionChoose(id, (long)sc.GetStudentSession()));
        }
        public ActionResult AppointmentEx()
        {
            return PartialView();
        }
        public string Exjson()
        {
            string pageIndex = Request["page"];
            string pageSize = Request["rows"];
            return new StudentOperation().Exjson(pageIndex, pageSize, (int)ExpermentType.Experment);
        }

        public ActionResult SeniorProject()
        {
            return PartialView();
        }

        public string GetSeniorProjectList(int page, int rows)
        {
            return new ExperimentOperation().GetSeniorProjectList((long)sc.GetStudentSession(), page, rows);
        }

        public JsonResult ExpermentProjectChoose(string id)
        {
            return Json(new ChooseOperation().ExpermentProjectChoose(id, (long)sc.GetStudentSession()));
        }

        public ActionResult MyProject(string parameter0)
        {
            ViewBag.type = parameter0;
            return PartialView();
        }

        public string MyProjectList(string id)
        {
            return new StudentOperation().MyProject(id, (long)sc.GetStudentSession()); ;
        }

        public ActionResult InnovationAndEnterprise()
        {
            return PartialView();
        }

        public string GetInnovationAndEnterpriseList(int page, int rows)
        {
            return new ExperimentOperation().GetInnovationAndEnterpriseList((long)sc.GetStudentSession(), page, rows);
        }
        public string TimeJson(string id)
        {
            return new ExperimentTimeOperation().GetJsonTime(id);

        }

        public ActionResult StudentMsg()
        {
            return PartialView();
        }

        public string GetPeopleMsg()
        {
            return new StudentOperation().GetPeopleMsg((long)sc.GetStudentSession());
        }
        public string CheckGrade(string id)
        {
            long sid = (long)sc.GetStudentSession();
            bool result = new ExperimentOperation().CheckGrade(id, sid);
            if (result)
            {
                return "{\"back\":\"ok\"}";
            }
            return "{\"back\":\"对不起，该实验不对您开放.\"}";

        }
        public ActionResult RePassword()
        {
            return PartialView();
        }

        public JsonResult RePasswordIn(string jpwd, string xpwd, string qpwd)
        {
            return Json(new StudentOperation().RePassword(jpwd, xpwd, qpwd, (long)sc.GetStudentSession()));
        }
        public string ChooseEx(string id, string exid, string dayOfWeek, string period)
        {

            long sid = (long)sc.GetStudentSession();
            string json = new ChooseOperation().ChooseEx(id, sid, exid, dayOfWeek, period);
            return "{\"back\":\"" + json + "\"}";
        }
        public ActionResult MyEx()
        {
            return PartialView();
        }
        public string MyExJson()
        {
            long id = (long)sc.GetStudentSession();

            return new ChooseOperation().MyExJson(id);

        }
    }
}
