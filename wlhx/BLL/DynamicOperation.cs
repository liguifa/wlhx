using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wlhx.Models;
using wlhx.Common;

namespace wlhx.BLL
{
    public class DynamicOperation : BaseBLL<Dynamic>
    {
        public List<Dynamic> GetOneShow(int type,bool isIndex=true)
        {
            List<Dynamic> ds = Search(u => u.dynamic_isIndex == isIndex && u.dynamic_class == type && u.dynamic_isDel == false, y => y.dynamic_publicTime, 1, 10);
            if (ds.Count > 0)
            {
                return ds;
            }
            return new List<Dynamic>();
        }
        public List<Dynamic> GetFileList(string Index, int pageSize, out int index, bool SoftwareCategory=false)
        {
            int pageMax = this.GetFilePageMax(pageSize);
            int pageIndex = 1;
            try
            {
                pageIndex = Convert.ToInt32(Index);
            }
            catch
            {
                pageIndex = 1;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageMax)
            {
                pageIndex = pageMax;
            }
            if (pageSize <= 0)
            {
                pageSize = 30;
            }
            index = pageIndex;
            return SoftwareCategory ? base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.File && (d.dynamic_softwareCategory == 1 || d.dynamic_softwareCategory == 2), d => d.dynamic_publicTime, pageIndex, pageSize) : base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.File, d => d.dynamic_publicTime, pageIndex, pageSize);
        }

        public int GetFilePageMax(int pageSize)
        {
            int fileSize = Convert.ToInt32(base.SearchCount(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.File));
            return fileSize % pageSize == 0 ? fileSize / pageSize : fileSize / pageSize + 1;
        }

        public List<Dynamic> GetExperimentList(string Index, int pageSize, out int index)
        {
            int pageMax = this.GetExperimentPageMax(pageSize);
            int pageIndex = 1;
            try
            {
                pageIndex = Convert.ToInt32(Index);
            }
            catch
            {
                pageIndex = 1;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageMax)
            {
                pageIndex = pageMax;
            }
            if (pageSize <= 0)
            {
                pageSize = 30;
            }
            index = pageIndex;
            return base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Experiment, d => d.dynamic_publicTime, pageIndex, pageSize);
        }

        public List<Dynamic> GetExperimentResourceList(string Index, int pageSize, out int index)
        {
            int pageMax = this.GetExperimentPageMax(pageSize);
            int pageIndex = 1;
            try
            {
                pageIndex = Convert.ToInt32(Index);
            }
            catch
            {
                pageIndex = 1;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageMax)
            {
                pageIndex = pageMax;
            }
            if (pageSize <= 0)
            {
                pageSize = 30;
            }
            index = pageIndex;
            return base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.ExperimentResource, d => d.dynamic_publicTime, pageIndex, pageSize);
        }

        public Dynamic GetDynamicMsg(long id, int type)
        {
            List<Dynamic> dc = base.Search(d => d.dynamic_isDel == false && d.dynamic_id == id && d.dynamic_class == type);
            return dc.Count != 0 ? dc[0] : new Dynamic() { dynamic_body = "", dynamic_title = "" };
        }

        public int GetExperimentPageMax(int pageSize)
        {
            int fileSize = Convert.ToInt32(base.SearchCount(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Experiment));
            return fileSize % pageSize == 0 ? fileSize / pageSize : fileSize / pageSize + 1;
        }

        public Dynamic GetTeamList()
        {
            List<Dynamic> Dynameics = base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.TeamInfo);
            return Dynameics.Count >= 1 ? Dynameics[0] : new Dynamic() { dynamic_body = "" };
        }

        public List<Dynamic> GetTeacherList(string Index, int pageSize, out int index)
        {
            int pageMax = this.GetExperimentPageMax(pageSize);
            int pageIndex = 1;
            try
            {
                pageIndex = Convert.ToInt32(Index);
            }
            catch
            {
                pageIndex = 1;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageMax)
            {
                pageIndex = pageMax;
            }
            if (pageSize <= 0)
            {
                pageSize = 30;
            }
            index = pageIndex;
            return base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Teacher, d => d.dynamic_publicTime, pageIndex, pageSize);
        }

        public int GetTeacherPageMax(int pageSize)
        {
            int fileSize = Convert.ToInt32(base.SearchCount(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Teacher));
            return fileSize % pageSize == 0 ? fileSize / pageSize : fileSize / pageSize + 1;
        }

        public List<Dynamic> GetCreateList(string Index, int pageSize, out int index)
        {
            int i = 0;
            int pageMax = this.GetCreatePageMax(pageSize, out i);
            int pageIndex = 1;
            try
            {
                pageIndex = Convert.ToInt32(Index);
            }
            catch
            {
                pageIndex = 1;
            }
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageIndex > pageMax)
            {
                pageIndex = pageMax;
            }
            if (pageSize <= 0)
            {
                pageSize = 30;
            }
            index = pageIndex;
            return base.Search(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Achievement, d => d.dynamic_publicTime, pageIndex, pageSize);
        }

        public int GetCreatePageMax(int pageSize, out int filesize)
        {
            int fileSize = Convert.ToInt32(base.SearchCount(d => d.dynamic_isDel == false && d.dynamic_class == (int)DynamicTypes.Achievement));
            filesize = fileSize;
            return fileSize % pageSize == 0 ? fileSize / pageSize : fileSize / pageSize + 1;
        }
        public string AddDynamic(string title, string body, bool isIndex, int type)
        {
            body = Common.Html.strToHtml(body);
            body = body.Replace("\n", "");
            body = body.Replace("\t", "");
            Dynamic d = new Dynamic();
            d.dynamic_body = body;
            d.dynamic_class = type;
            d.dynamic_isDel = false;
            d.dynamic_publicTime = DateTime.Now;
            d.dynamic_title = title;
            d.dynamic_isIndex = isIndex;
            try
            {
                Add(d);
            }
            catch
            {
                return "保存出错，请检查数据，稍后再试。";
            }
            return "ok";
        }

        public JsonStatus AddFileIn(string title, string url,string iskj=null,string kjclass=null)
        {
            JsonStatus js = new JsonStatus();

            try
            {
                Dynamic d = iskj == null ? new Dynamic()
                {
                    dynamic_id = 1,
                    dynamic_isDel = false,
                    dynamic_isIndex = false,
                    dynamic_body = url,
                    dynamic_title = title,
                    dynamic_publicTime = DateTime.Now,
                    dynamic_class = (int)DynamicTypes.File
                } : new Dynamic() 
                {
                    dynamic_id = 1,
                    dynamic_isDel = false,
                    dynamic_isIndex = false,
                    dynamic_body = url,
                    dynamic_title = title,
                    dynamic_publicTime = DateTime.Now,
                    dynamic_class = (int)DynamicTypes.File,
                    dynamic_softwareCategory = kjclass=="视频"?1:2
                };
                base.Add(d);
                js.status = "1";
                js.msg = "添加成功！";
            }
            catch
            {
                js.status = "0";
                js.msg = "添加失败！未知错误.....";
            }
            return js;
        }

        public string GetDyamicListJson(int type, int pageIndex, int pageSize)
        {
            string json = "{\"total\":\"" + this.GetDynamicSize(type) + "\",\"rows\":[";

            List<Dynamic> dynamics = base.Search(d => d.dynamic_isDel == false && d.dynamic_class == type, d => d.dynamic_publicTime, pageIndex, pageSize);
            foreach (Dynamic dynamic in dynamics)
            {
                string html = CheckStr.checkStr(dynamic.dynamic_body);
                html = html.Length >= 100 ? html.Substring(0, 100) : html;
                json = json + "{\"id\":\"" + dynamic.dynamic_id + "\",\"title\":\"" + dynamic.dynamic_title + "\",\"body\":\"" + html + "\",\"index\":\"" + this.isIndex(dynamic.dynamic_isIndex, type) + "\",\"time\":\"" + dynamic.dynamic_publicTime + "\"},";
            }
            json = json.Substring(0, json.Length - 1) + "]}";
            return json;
        }

        public string isIndex(object isIndex, int type)
        {
            if (type == (int)DynamicTypes.Toast || type == (int)DynamicTypes.News)
            {
                bool index = isIndex == null ? false : (bool)isIndex;
                return index ? "是" : "否";
            }
            else
            {
                return "--";
            }
        }
        public string EditDynamic(string title, string body, bool isIndex, int type, long id)
        {
            body = Common.Html.strToHtml(body);

            Dynamic d = Search(u => u.dynamic_id == id)[0];
            d.dynamic_body = body;
            d.dynamic_class = type;
            d.dynamic_isDel = false;
            d.dynamic_publicTime = DateTime.Now;
            d.dynamic_title = title;
            d.dynamic_isIndex = isIndex;
            try
            {
                Modify(d, "dynamic_body", "dynamic_class", "dynamic_publicTime", "dynamic_title", "dynamic_isIndex");
            }
            catch
            {
                return "保存出错，请检查数据，稍后再试。";
            }
            return "ok";

        }

        public JsonStatus DynamicRemove(string id)
        {
            JsonStatus js = new JsonStatus();
            long dId = 0;
            try
            {
                dId = Convert.ToInt64(id);
                Dynamic d = base.Search(u => u.dynamic_id == dId && u.dynamic_isDel == false)[0];
                d.dynamic_isDel = true;
                base.Modify(d, "dynamic_isDel");
                js.status = "1";
                js.msg = "删除成功！";
            }
            catch
            {
                js.status = "0";
                js.msg = "删除失败！数据异常.....";
            }
            return js;
        }

        public long GetDynamicSize(int type)
        {
            return base.SearchCount(d => d.dynamic_isDel == false && d.dynamic_class == type);
        }
        public string ModifyFile(string id, string title)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(title))
            {
                return "数据不可为空！";
            }
            long fileId = Convert.ToInt64(id);
            Dynamic d = Search(u => u.dynamic_id == fileId)[0];
            d.dynamic_title = title;
            try
            {
                Modify(d, "dynamic_title");
            }
            catch
            {
                return "未知错误，请检查数据重试";
            }
            return "ok";
        }

        public List<Dynamic> GetIndexNewsAndToast()
        {
            return base.Search(d => d.dynamic_isDel == false && d.dynamic_isIndex == true&&(d.dynamic_class==(int)DynamicTypes.News||d.dynamic_class==(int)DynamicTypes.Toast), d => d.dynamic_publicTime, 1, 4);
        }
    }
}