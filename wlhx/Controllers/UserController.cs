using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Verification;
using wlhx.BLL;
using wlhx.Common;
using wlhx.Models;

namespace wlhx.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        #region 包起来
        public static SessionController sc = null;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AdminManager()
        {
            return PartialView();
        }

        public string GetAdminJson()
        {
            return new AdminOperation().GetAdminJson();
        }
        public ActionResult AddNotice(string parameter0)
        {
            ViewBag.type = parameter0;
            return PartialView();
        }

        public ActionResult Editors(int id, string type)
        {
            ViewBag.type = id;
            ViewBag.msg = String.IsNullOrEmpty(type) ? null : new DynamicOperation().GetDynamicMsg(Convert.ToInt32(type), Convert.ToInt32(id));
            ViewBag.isEdit = String.IsNullOrEmpty(type) ? false : true;
            return PartialView();
        }

        public JsonResult ModifyAdmin(string id, string user, string level)
        {
            return Json(new AdminOperation().ModifyAdmin(id, user, level));
        }

        public JsonResult AddAdmin(string user, string level)
        {
            return Json(new AdminOperation().AddAdmin(user, level));
        }

        public ActionResult RePassword()
        {
            return PartialView();
        }
        public JsonResult RePasswordIn(string jpwd, string xpwd, string qpwd)
        {
            return Json(new AdminOperation().RePasswordIn(Convert.ToInt64(sc.GetUserSession()), jpwd, xpwd, qpwd));
        }

        public string UploadJson()
        {
            HttpPostedFileBase image = Request.Files["imgFile"];

            string path = Server.MapPath("/upload");
            string json = new SaveFile().SaveImageTo(path, image); ;
            return json;
        }

        public ActionResult AddFile()
        {
            return PartialView();
        }

        public JsonResult AddFileIn(string title, HttpPostedFileBase file, string iskj = null, string kjclass = null)
        {
            string path = Server.MapPath("~/file");
            path = new SaveFile().SaveFileTo(path, file);
            return path == "标题或文件不能为空....." ? Json(new JsonStatus() { status = "0", msg = "标题或文件不能为空....." }) : Json(new DynamicOperation().AddFileIn(title, path, iskj, kjclass));
        }
        [ValidateInput(false)]
        public string AddDynamic()
        {
            string title = Request["title"];
            string body = Request["body"];
            string isIndex = Request["isIndex"];
            bool isI = false;
            int type = Convert.ToInt32(Request["type"]);
            string isEdit = Request["isEdit"];

            if (isIndex == "true")
            {
                isI = true;
            }
            string json = null;
            if (isEdit == "False")
            {
                json = new DynamicOperation().AddDynamic(title, body, isI, type);
            }
            else
            {
                if (Request["id"] == null)
                {
                    return "{\"back\":\"未知错误，请重试！\"}";
                }
                long id = Convert.ToInt64(Request["id"]);
                json = new DynamicOperation().EditDynamic(title, body, isI, type, id);
            }
            return "{\"back\":\"" + json + "\"}";
        }

        public ActionResult DynamicList(string parameter0)
        {
            ViewBag.type = parameter0;
            return PartialView();
        }

        public string GetDyamicListJson(int id, int pageIndex, int pageSize, int page, int rows)
        {
            pageIndex = page == null ? pageIndex : page;
            pageSize = rows == null ? pageSize : rows;
            return new DynamicOperation().GetDyamicListJson(id, pageIndex, pageSize);
        }

        [HttpPost]
        public JsonResult DynamicRemove(string id)
        {
            return Json(new DynamicOperation().DynamicRemove(id));
        }

        public ActionResult FileList(string parameter0)
        {
            ViewBag.type = parameter0;
            return PartialView();
        }
        public string MofifyFile()
        {
            string id = Request["id"];
            string title = Request["title"];
            string json = new DynamicOperation().ModifyFile(id, title);
            return "{\"back\":\"" + json + "\"}";
        }

        public ActionResult VerCode()
        {
            VerificationCode vc = new VerificationCode();
            Session["ver"] = vc.GetVerCode();
            return File(vc.StreamVerCod(), @"image/jpeg");
        }

        public JsonResult LoginIn(string user, string pwd, string yzm)
        {
            long id = 0;
            JsonStatus js = new AdminOperation().Login(user, pwd, yzm, (string)Session["ver"], out id);
            if (js.status == "1")
            {
                sc.SetUserSession(id);
            }
            return Json(js);
        }

        public ActionResult StudentManagement()
        {
            ViewBag.proDir = new ProfessionalOperation().GetProfessionalList();
            ViewBag.s_class = new StudentOperation().GetStudentClass();
            return PartialView();
        }

        public string GetStudentList(int gade, int page, int rows, int professional = 0, string s_class = "100")
        {
            return new StudentOperation().GetStudentList(page, rows, gade, professional, s_class);
        }

        public ActionResult ExperimentMangement()
        {

            return PartialView();
        }

        public JsonResult InputStudent(HttpPostedFileBase file)
        {
            return Json(new StudentOperation().InputStudent(Server.MapPath("~/outExecl"), file));
        }
        public string JsonExperiment()
        {
            string pageIndex = Request["page"];
            string pageSize = Request["rows"];
            return new ExperimentOperation().JsonExperiment(pageIndex, pageSize, (int)ExpermentType.Experment);

        }
        public string JsonCreat()
        {
            string pageIndex = Request["page"];
            string pageSize = Request["rows"];
            return new ExperimentOperation().JsonExperiment(pageIndex, pageSize, (int)ExpermentType.Creativity);
        }
        public JsonResult AddStudent(string student_num, string name, string grade, string professional, string _class, string proDir)
        {
            return Json(new StudentOperation().AddStudent(student_num, name, grade, professional, _class, proDir));
        }

        public JsonResult EditStudent(string id, string student_num, string name, string grade, string professional, string _class, string proDir)
        {
            return Json(new StudentOperation().EditStudent(id, student_num, name, grade, professional, _class, proDir));
        }

        public JsonResult RemoveStudent(string id)
        {
            return Json(new StudentOperation().RemoveStudent(id));
        }

        public ActionResult StudentDetailedMsg(string id)
        {
            ViewBag.id = id;
            return PartialView();
        }
        public string StudentDetailedMsgList(string id)
        {
            return new StudentOperation().StudentDetailedMsgList(id);
        }

        public JsonResult ResetStudentPassword(string id)
        {
            return Json(new StudentOperation().ResetStudentPassword(id));
        }

        public JsonResult RemoveStudentChoose(string id)
        {
            return Json(new ChooseOperation().RemoveStudentChoose(id));
        }
        public string RemoveExperiment(string id)
        {
            string json = new ExperimentOperation().RemoveExperiment(id);
            return "{\"back\":\"" + json + "\"}";
        }
        public string AddExperiment()
        {
            string name = Request["name"];
            string allowGrade = Request["allGrade"];
            string week = Request["week"];
            string json = new ExperimentOperation().AddExperiment(name, allowGrade, (int)ExpermentType.Experment, 10000, week);
            return "{\"back\":\"" + json + "\"}";
        }
        public string AddCreat()
        {
            string name = Request["name"];
            string allowGrade = Request["allGrade"];
            int max = Convert.ToInt32(Request["max"]);
            string week = Request["week"];
            string json = new ExperimentOperation().AddExperiment(name, allowGrade, (int)ExpermentType.Creativity, max, week);
            return "{\"back\":\"" + json + "\"}";
        }
        public string EditExperiment()
        {
            string id = Request["id"];
            string name = Request["name"];
            string allowGrade = Request["allGrade"];
            string week = Request["week"];
            string json = new ExperimentOperation().EditExperiment(id, name, allowGrade, "10000", week);
            return "{\"back\":\"" + json + "\"}";
        }
        public string EditCreatity()
        {
            string id = Request["id"];
            string name = Request["name"];
            string allowGrade = Request["allGrade"];
            string max = Request["max"];
            string week = Request["week"];
            string json = new ExperimentOperation().EditExperiment(id, name, allowGrade, max, week);
            return "{\"back\":\"" + json + "\"}";
        }

        public ActionResult ProfessionalEmphasisManagement()
        {
            return PartialView();
        }

        public string GetProfessionalList()
        {
            return new ProfessionalOperation().GetProfessionalList();
        }

        public ActionResult GetProfessionStudent(string id)
        {
            ViewBag.id = id;
            return PartialView();
        }

        public JsonResult ModifyStudentFromProfessional(string mark, string student_num, long professionalId)
        {
            return Json(new StudentOperation().ModifyStudentFromProfessional(mark, student_num, professionalId));
        }
        public string TimeJson(string id)
        {
            return new ExperimentTimeOperation().GetJsonTime(id);

        }
        public string AddExperimentTime()
        {
            string exid = Request["exid"];
            string week = Request["week"];
            string time = Request["time"];
            string max = Request["max"];
            string json = new ExperimentTimeOperation().AddExperimentTime(exid, week, time, max);
            return "{\"back\":\"" + json + "\"}";
        }
        public string EditExperimentTime()
        {
            string id = Request["id"];
            string week = Request["week"];
            string time = Request["time"];
            string max = Request["max"];
            string json = new ExperimentTimeOperation().EditExperiment(id, week, time, max);
            return "{\"back\":\"" + json + "\"}";
        }

        public JsonResult OutStudent(long proDirId)
        {
            return Json(new StudentOperation().OutStudent(Server.MapPath("~/outExecl"), proDirId));
        }
        public FileResult GetExcel(string id)
        {
            string name = System.IO.Path.GetFileName(id + ".xlsx");
            return File(Server.MapPath("~/outExecl") + "/" + id + ".xlsx", "application/octet-stream", name.Split('@')[name.Split('@').Length - 1]);
        }
        public string DelExperiment(long id)
        {
            string json = new ExperimentTimeOperation().DelExperiment(id);
            return "{\"back\":\"" + json + "\"}";
        }
        public ActionResult LookForStudent(string id)
        {
            ViewBag.exid = id;
            ViewBag.Time = new ChooseOperation().GetExperimentTime(Convert.ToInt64(id));
            return PartialView();
        }

        public JsonResult AddProfessional(string title)
        {
            return Json(new ProfessionalOperation().AddProfessional(title));
        }

        public JsonResult EditProfessional(long id, string title)
        {
            return Json(new ProfessionalOperation().EditProfessional(id, title));
        }

        public JsonResult RemoveProfessional(long id)
        {
            return Json(new ProfessionalOperation().RemoveProfessional(id));
        }
        public string GetStudentJson(string id, string time = "全部")
        {

            return new StudentOperation().GetStudentJson(id, true, time);
        }
        public string GetCreStudentJson(string id)
        {

            return new StudentOperation().GetStudentJson(id, false, null);
        }

        public string GetSenioProjectList(int page, int rows)
        {
            return new ExperimentOperation().GetSeniorProjectList(0, page, rows);
        }

        public ActionResult SeniorProjectManagement()
        {
            return PartialView();
        }
        #endregion

        public ActionResult GetSenionrProjectStudent(string id)
        {
            ViewBag.id = id;
            return PartialView();
        }
        public string GetStudentListFromSeniorProject(string projectId, int page, int rows)
        {
            return new ExperimentOperation().GetStudentListFromSeniorProject(projectId, page, rows);
        }
        public string GetStudetFile(string id, string time = "全部")
        {
            string path = Server.MapPath("~/outExecl");
            string filePath = new ChooseOperation().GetStudetFile(id, path, true, time);

            return "{\"back\":\"" + filePath + "\"}";
        }
        public string GetCreStudentFile(string id)
        {
            string path = Server.MapPath("~/outExecl");
            string filePath = new ChooseOperation().GetStudetFile(id, path, false, null);

            return "{\"back\":\"" + filePath + "\"}";
        }
        public ActionResult CreatNew()
        {
            return PartialView();
        }

        public JsonResult OutStudentFromProject(long id)
        {
            return Json(new ExperimentOperation().OutStudentFromProject(id, Server.MapPath("~/outExecl")));
        }

        public JsonResult ModifyStudentFromProject(string mark, string student_num, long ProjectId)
        {
            return Json(new ChooseOperation().ModifyStudentFromProject(mark, student_num, ProjectId));
        }
        public ActionResult CreatStudent(string id)
        {
            ViewBag.cid = id;
            return PartialView();
        }
        public string AddExperimentChooseStu(string id, string time)
        {
            string exid = Request["exid"];
            string json = new StudentOperation().AddExperimentChooseStu(id, exid, time);
            return "{\"back\":\"" + json + "\"}";
        }
        public string AddChooseStu(string id)
        {
            string exid = Request["exid"];
            string json = new StudentOperation().AddChooseStu(id, exid);
            return "{\"back\":\"" + json + "\"}";
        }
        public string DelChooseStu(string id)
        {
            string exid = Request["exid"];
            string json = new StudentOperation().DelChoose(id, exid);
            return "{\"back\":\"" + json + "\"}";
        }
        public string DelExtimeChooseStu(string id)
        {
            string exid = Request["exid"];
            string json = new StudentOperation().DelExtimeChooseStu(id, exid);
            return "{\"back\":\"" + json + "\"}";
        }
        public JsonResult AddProjectFromSenior(string id, string title, string grade, string totalNum, string type, string src, string teacher)
        {
            return Json(new ExperimentOperation().AddProjectFromSenior(id, title, grade, totalNum, type, src, teacher));
        }

        public JsonResult EditProjectFromSenior(string id, string title, string grade, string totalNum, string type, string src, string teacher)
        {
            return Json(new ExperimentOperation().EditProjectFromSenior(id, title, grade, totalNum, type, src, teacher));
        }

        public JsonResult RemoveProjectFromSenior(string id)
        {
            return Json(new ExperimentOperation().RemoveProjectFromSenior(id));
        }

        public ActionResult ClearSystem()
        {
            return PartialView();
        }

        public JsonResult ClearSystemIn()
        {
            return Json(new DataProcessing().ClearSystemIn(Server.MapPath("~/outExecl")));
        }
    }
}
