using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wlhx.BLL;
using wlhx.Common;

namespace wlhx.Controllers
{
    [OutputCache(Duration=60)]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            DynamicOperation d = new DynamicOperation();
            ViewBag.indeNews = d.GetIndexNewsAndToast();
            ViewBag.showNotice = d.GetOneShow((int)DynamicTypes.Toast);
            ViewBag.showNews = d.GetOneShow((int)DynamicTypes.News);
            ViewBag.showCG = d.GetOneShow((int)DynamicTypes.Achievement,false);
            return View();
        }
        #region  public ActionResult Dynamic(string id)

        public ActionResult Dynamic(string id)
        {
            string pageIndex = id;
            int pageNum = 0;
            if (string.IsNullOrEmpty(pageIndex))
            {
                pageNum = 1;
            }
            else
            {
                pageNum = Convert.ToInt32(pageIndex);
            }
            long totalNum = 0;

            ViewBag.newsList = new News().GetNewsList(pageNum, 10, out totalNum, (int)DynamicTypes.News);
            long totalPage = totalNum / 10;
            if (totalNum % 10 != 0)
            {
                totalPage++;
            }
            if (totalPage < pageNum)
            {
                pageNum = (int)totalPage;
            }
            if (pageNum == 0)
            {
                pageNum = 1;
            }
            ViewBag.i = 0;
            ViewBag.totalPage = totalPage;
            ViewBag.totalNum = totalNum;
            ViewBag.pageIndex = pageNum;
            return View();
        }

        #endregion
        public ActionResult General()
        {
            return View();
        }
        public ActionResult About(string id)
        {
            DynamicOperation doper = new DynamicOperation();
            ViewBag.teamInfo = doper.GetTeamList();
            int index = 1;
            ViewBag.teacherlist = doper.GetTeacherList(id, 12, out index);
            ViewBag.index = index;
            ViewBag.listsize = doper.GetTeacherPageMax(12);
            ViewBag.isPrevious = index == 1 ? false : true;
            ViewBag.isNext = ViewBag.listsize == index ? false : true;
            return View();
        }
        public ActionResult CreativeExercise(string id)
        {
            DynamicOperation doper = new DynamicOperation();
            int index = 1;
            ViewBag.createlist = doper.GetCreateList(id, 12, out index);
            ViewBag.index = index;
            int filesize = 0;
            ViewBag.listsize = doper.GetCreatePageMax(12, out filesize);
            ViewBag.isPrevious = index == 1 ? false : true;
            ViewBag.isNext = ViewBag.listsize == index ? false : true;
            ViewBag.size = filesize;
            return View();
        }

        public ActionResult ExperimentContent(string id)
        {
            DynamicOperation doper = new DynamicOperation();
            int index = 1;
            ViewBag.filelist = doper.GetExperimentList(id, 30, out index);
            ViewBag.index = index;
            ViewBag.isPrevious = index == 1 ? false : true;
            ViewBag.isNext = doper.GetExperimentPageMax(30) == index ? false : true;
            return View();
        }

        public ActionResult ExperimentResource(string pageIndex)
        {
            DynamicOperation doper = new DynamicOperation();
            int index = 0;
            ViewBag.resList = doper.GetExperimentResourceList(pageIndex, 30, out index);
            return View();
        }

        public ActionResult FileDown(string id)
        {
            DynamicOperation doper = new DynamicOperation();
            int index = 1;
            ViewBag.filelist = doper.GetFileList(id, 30, out index);
            ViewBag.index = index;
            ViewBag.isPrevious = index == 1 ? false : true;
            ViewBag.isNext = doper.GetFilePageMax(30) == index ? false : true;
            return View();
        }

        public FileResult ResposeFile(string id)
        {
            string file = new DynamicOperation().GetDynamicMsg(Convert.ToInt32(id), (int)DynamicTypes.File).dynamic_body;
            string name = System.IO.Path.GetFileName(file);
            return File(file, "application/octet-stream", name);
        }

        public ActionResult See(string id, string type, string liid)
        {
            ViewBag.msg = new DynamicOperation().GetDynamicMsg(Convert.ToInt32(id), Convert.ToInt32(type));
            ViewBag.id = "#" + liid;
            return View();
        }
        public ActionResult Notice(string id)
        {
            string pageIndex = id;
            int pageNum = 0;
            if (string.IsNullOrEmpty(pageIndex))
            {
                pageNum = 1;
            }
            else
            {
                pageNum = Convert.ToInt32(pageIndex);
            }
            long totalNum = 0;

            ViewBag.newsList = new News().GetNewsList(pageNum, 10, out totalNum, (int)DynamicTypes.Toast);
            long totalPage = totalNum / 10;
            if (totalNum % 10 != 0)
            {
                totalPage++;
            }
            if (totalPage < pageNum)
            {
                pageNum = (int)totalPage;
            }
            if (pageNum == 0)
            {
                pageNum = 1;
            }
            ViewBag.i = 0;
            ViewBag.totalPage = totalPage;
            ViewBag.totalNum = totalNum;
            ViewBag.pageIndex = pageNum;
            return View();
        }

        public ActionResult BrowserError()
        {
            return View();
        }

        public ActionResult SoftwareCategory(string id, int pageSize=30)
        {
            DynamicOperation doper = new DynamicOperation();
            int index = 1;
            ViewBag.filelist = doper.GetFileList(id, pageSize, out index,true);
            ViewBag.index = index;
            ViewBag.isPrevious = index == 1 ? false : true;
            ViewBag.isNext = doper.GetFilePageMax(30) == index ? false : true;
            return View();
        }

        public ActionResult ShowVideo(string id, string type, string liid)
        {
            ViewBag.msg = new DynamicOperation().GetDynamicMsg(Convert.ToInt32(id), Convert.ToInt32(type));
            ViewBag.id = "#" + liid;
            return View();
        }

        public ActionResult ShowPPT(string id, string type, string liid)
        {
            ViewBag.msg = new DynamicOperation().GetDynamicMsg(Convert.ToInt32(id), Convert.ToInt32(type));
            ViewBag.id = "#" + liid;
            return View();
        }
    }
}
