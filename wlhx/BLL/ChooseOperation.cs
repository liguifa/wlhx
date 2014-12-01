using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using wlhx.Common;
using wlhx.DAL;
using wlhx.Models;

namespace wlhx.BLL
{
    public class ChooseOperation : BaseBLL<Choos>
    {
        public JsonStatus ExpermentProjectChoose(string expirmentId, long studentId)
        {
            JsonStatus js = new JsonStatus();
            List<Student> students = new StudentOperation().Search(m => m.student_id == studentId && m.student_isDel == false);
            long pId = 0;
            try
            {
                pId = Convert.ToInt64(expirmentId);
                List<Experiment> experiments = new ExperimentOperation().Search(d => d.experiment_isDel == false && d.experiment_id == pId);
                List<Choos> chooses = base.Search(d => d.choose_ownStudentId == studentId && d.choose_isDel == false && d.choose_ownExperimentId == pId);
                if (students.Count >= 1 && experiments.Count >= 1)
                {
                    if (experiments[0].experiment_allowGrades.Split(',').Contains(students[0].student_grade.ToString()))
                    {
                        if (experiments[0].experiment_peopleNum > base.SearchCount(d => d.choose_isDel == false && d.choose_ownExperimentId == pId))
                        {
                            if (chooses.Count == 0 && ((experiments[0].experiment_class == (int)ExpermentType.Graduate && base.SearchCount(d => d.choose_ownStudentId == studentId && d.choose_isDel == false && d.Experiment.experiment_class == (int)ExpermentType.Graduate) == 0) || experiments[0].experiment_class != (int)ExpermentType.Graduate))
                            {
                                try
                                {
                                    Choos c = new Choos()
                                    {
                                        choose_id = 1,
                                        choose_isDel = false,
                                        choose_ownExperimentId = pId,
                                        choose_ownStudentId = studentId,
                                    };
                                    base.Add(c);
                                    js.status = "1";
                                    js.msg = "选择成功！";
                                }
                                catch
                                {
                                    js.status = "0";
                                    js.msg = "选择失败！未知错误.....";
                                }
                            }
                            else
                            {
                                js.status = "0";
                                js.msg = "你已选择！不能再次选择！如要修改请联系管理员.....";
                            }
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "选择失败！人数已满.....";
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "选择失败！你现在无权选择.....";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "选择失败！数据错误.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "选择失败！数据错误.....";
            }

            return js;
        }
        public string ChooseEx(string id, long stuid, string exsid, string dayOfWeek, string period)
        {
            long exid = Convert.ToInt64(exsid);
            long extid = Convert.ToInt64(id);
            long count = SearchCount(u => u.choose_ownStudentId == stuid && u.choose_isDel == false && u.choose_ownExperimentId == exid);
            if (count > 0)
            {
                return "您已选择该实验，不要重复选择";
            }
            Choos c = new Choos();
            c.choose_isDel = false;
            c.choose_ownExperimentId = exid;
            c.choose_ownStudentId = stuid;
            c.choose_ExperimenTimet = dayOfWeek + period;
            Add(c);

            return "ok";
        }
        public string MyExJson(long sid)
        {
            List<Choos> cs = Search(u => u.choose_ownStudentId == sid && u.choose_isDel == false);
            if (cs.Count < 1)
            {
                return "您还没有预约实验呢.";
            }
            StringBuilder sb = new StringBuilder();
            string s = "{\"total\":" + cs.Count + ",\"rows\":[";
            sb.Append(s);
            foreach (Choos c in cs)
            {
                s = "{\"title\":\"" + c.Experiment.experiment_title + "\",\"week\":\"\",\"time\":\"" + c.choose_ExperimenTimet + "\",\"max\":\"\"},";
                sb.Append(s);
            }
            if (cs.Count != 0)
            {
                s = sb.ToString();
                s = s.Substring(0, s.Length - 1);
            }
            return s + "]}";
        }

        public JsonStatus RemoveStudentChoose(string id)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                long studentId = Convert.ToInt64(id);
                List<Choos> chooses = base.Search(d => d.choose_id == studentId && d.choose_isDel == false);
                if (chooses.Count >= 1)
                {
                    chooses[0].choose_isDel = true;
                    base.Modify(chooses[0], "choose_isDel");
                    js.status = "1";
                    js.msg = "删除成功！";
                }
                else
                {
                    js.status = "0";
                    js.msg = "删除失败！数据异常！";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "删除失败！数据异常！";
            }
            return js;
        }
        public string GetStudetFile(string id, string path, bool isExtime,string time)
        {
            long exid = Convert.ToInt64(id);
            List<Choos> cs = new List<Choos>();
            if (time == "全部")
            {
                cs = Search(u => u.choose_ownExperimentId == exid && u.choose_isDel == false);
            }
            else
            {
                cs = Search(u => u.choose_ExperimenTimet == time && u.choose_isDel == false && u.choose_ownExperimentId == exid);
            }
            List<Student> stus = new List<Student>(cs.Count);
            for (int i = 0; i < cs.Count; i++)
            {
                stus.Add(cs[i].Student);
            }

            JsonStatus js = DataProcessing.OutExecl(path, new StudentOperation().ListToDataTable(stus), new string[] { "学号", "姓名", "年级", "专业", "班级", "专业方向" }, "学生名单");
            return js.msg;
        }
        public JsonStatus ModifyStudentFromProject(string mark, string student_num, long projectId)
        {
            JsonStatus js = new JsonStatus();
            List<Student> students = new StudentOperation().Search(d => d.student_number == student_num && d.student_isDel == false);
            List<Experiment> experiments = new ExperimentOperation().Search(d => d.experiment_isDel == false && d.experiment_id == projectId);
            if (experiments.Count >= 1)
            {
                if (students.Count >= 1)
                {
                    switch (mark)
                    {
                        case "add":
                            {
                                if (base.SearchCount(d => d.Student.student_number == student_num && d.choose_isDel == false && d.Student.student_isDel == false) <= 0)
                                {
                                    Choos c = new Choos()
                                    {
                                        choose_id = 1,
                                        choose_isDel = false,
                                        choose_ownExperimentId = projectId,
                                        choose_ownStudentId = students[0].student_id
                                    };
                                    base.Add(c);
                                    js.status = "1";
                                    js.msg = "添加成功！";
                                }
                                else
                                {
                                    js.status = "0";
                                    js.msg = "添加失败！该学生已选择另一课题如要添加请先在另一表中删除！";
                                }
                                break;
                            }
                        case "remove":
                            {
                                List<Choos> chooses = base.Search(d => d.choose_isDel == false && d.Student.student_number == student_num && d.choose_ownExperimentId == projectId && d.choose_isDel == false);
                                if (chooses.Count >= 1)
                                {
                                    chooses[0].choose_isDel = true;
                                    base.Modify(chooses[0], "choose_isDel");
                                    js.status = "1";
                                    js.msg = "删除成功！";
                                }
                                else
                                {
                                    js.status = "0";
                                    js.msg = "删除失败！数据异常！";
                                }
                                break;
                            }
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = mark == "add" ? "输入的学号不存在！请检查输入是否正确！" : "数据异常！";
                }
            }
            else
            {
                js.status = "0";
                js.msg = "操作失败！数据异常！";
            }
            return js;
        }
        public string GetExperimentTime(long expId)
        {
            List<string> os=base.Search(d => d.choose_ownExperimentId == expId && d.choose_isDel == false).Select(d => d.choose_ExperimenTimet).Distinct().ToList();
            string options = "<option>全部</option>";
            foreach (string option in os)
            {
                options += "<option>" + option + "</option>";
            }
            return options;
        }
    }
}