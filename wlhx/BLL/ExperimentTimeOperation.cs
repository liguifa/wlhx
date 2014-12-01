using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using wlhx.DAL;
using wlhx.Models;

namespace wlhx.BLL
{
    public class ExperimentTimeOperation : BaseBLL<ExperimentTime>
    {
        public string GetJsonTime(string exId)
        {

            if (string.IsNullOrEmpty(exId))
            {
                return "error";
            }
            long id = Convert.ToInt64(exId);
            List<Experiment> exs =new Experiments().Search(j => j.experiment_id==id);
            StringBuilder sb = new StringBuilder();
            string s = "{\"total\":" + exs.Count + ",\"rows\":[";
            sb.Append(s);
            int index = 0;
            foreach (Experiment e in exs)
            {
                string timeID = e.experiment_week;
                long count = new ChooseOperation().SearchCount(k => k.choose_ownExperimentId == id);
                s = "{\"id\":\"" + e.experiment_id + "\",\"week\":\"" + e.experiment_week + "\",\"num\":\"" + count + "\",\"max\":\"" + e.experiment_peopleNum + "\",\"timebtn\":\"<button name=" + (index++) + "  class='timebtn' value='" + e.experiment_id + "'>选择 </button>\"},";
                sb.Append(s);
            }
            if (exs.Count != 0)
            {
                s = sb.ToString();
                s = s.Substring(0, (s.Length - 1));
            }
            return s + "]}";
        }
        public string AddExperimentTime(string id, string week, string time, string max)
        {

            if (string.IsNullOrEmpty(week) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(max) || string.IsNullOrEmpty(id))
            {
                return "不允许有空值，请检查输入！";
            }
            long exid = Convert.ToInt64(id);
            ExperimentTime et = new ExperimentTime();
            et.experimentTime_isDel = false;
            et.experimentTime_ownExperimentId = exid;
            et.experimentTime_peopleNum = Convert.ToInt32(max);
            et.experimentTime_startTime = time;
            et.experimentTime_week = week;
            try
            {
                Add(et);
            }
            catch
            {
                return "添加失败，未知错误";
            }
            return "ok";
        }
        public string EditExperiment(string id, string week, string time, string max)
        {

            if (string.IsNullOrEmpty(week) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(max) || string.IsNullOrEmpty(id))
            {
                return "不允许有空值，请检查输入！";
            }
            long etid = Convert.ToInt64(id);
            ExperimentTime e = Search(u => u.experimentTime_id == etid)[0];
            e.experimentTime_week = week;
            e.experimentTime_startTime = time;
            e.experimentTime_peopleNum = Convert.ToInt32(max);
            Modify(e, "experimentTime_week", "experimentTime_startTime", "experimentTime_peopleNum");
            return "ok";
        }
        public string DelExperiment(long id)
        {
            try
            {
                ExperimentTime e = Search(u => u.experimentTime_id == id)[0];
                e.experimentTime_isDel = true;
                Modify(e, "experimentTime_isDel");
            }
            catch
            {
                return "删除失败";
            }
            return "ok";
        }
    }
}