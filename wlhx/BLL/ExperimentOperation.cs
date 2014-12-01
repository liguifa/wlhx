using BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using wlhx.Common;
using wlhx.Models;

namespace wlhx.BLL
{
    public class ExperimentOperation : BaseBLL<Experiment>
    {
        public string GetSeniorProjectList(long id, int pageIndex, int pageSize)
        {
            StringBuilder json = new StringBuilder();

            List<Student> students = new StudentOperation().Search(d => d.student_id == id && d.student_isDel == false);
            Student s = students.Count >= 1 ? students[0] : new Student();
            List<Experiment> experiments = base.Search(d => d.experiment_isDel == false && d.experiment_class == (int)ExpermentType.Graduate, d => d.experiment_id, pageIndex, pageSize);
            json.Append("{\"total\":\"" + experiments.Count + "\",\"rows\":[");
            foreach (Experiment experiment in experiments)
            {
                json.Append("{\"id\":\"" + experiment.experiment_id + "\",\"title\":\"" + experiment.experiment_title + "\",\"grade\":\"" + experiment.experiment_allowGrades + "\",\"chooseNum\":\"" + experiment.Chooses.Count(d => d.choose_ownExperimentId == experiment.experiment_id) + "\",\"totalNum\":\"" + experiment.experiment_peopleNum + "\",\"oper\":\"" + GetStudentOperatioon(experiment.experiment_peopleNum, s, experiment.Chooses.Count(d => d.choose_isDel == false), experiment.experiment_id, experiment.experiment_allowGrades.Split(','), true) + "\"},");
            }
            string res = json.ToString();
            res = res.Length >= 2 ? res.Substring(0, res.Length - 1) + "]}" : res;
            return res;
        }



        private string GetStudentOperatioon(int people_number, Student s, int choosePeopleNumber, long experimentId, string[] allowGrade, bool isLimit)
        {
            bool isChoose = new ChooseOperation().SearchCount(d => d.choose_ownExperimentId == experimentId && d.choose_isDel == false && d.choose_ownStudentId == s.student_id) >= 1;
            bool isHave = isLimit ? new ChooseOperation().SearchCount(d => d.choose_isDel == false && d.choose_ownStudentId == s.student_id && d.Experiment.experiment_class == (int)ExpermentType.Graduate) >= 1 : false;
            return isLimit ? (s == null ? "---" : (isChoose ? "已选择" : (allowGrade.Contains(s.student_grade.ToString()) ? choosePeopleNumber >= people_number ? "人数已满" : (isHave ? "---" : "<button class='choose' value='" + experimentId + "'>选择</button>") : "---"))) : (s == null ? "---" : (isChoose ? "已选择" : (allowGrade.Contains(s.student_grade.ToString()) ? (choosePeopleNumber >= people_number ? "人数已满" : "<button class='choose' value='" + experimentId + "'>选择</button>") : "---")));
        }

        public string GetInnovationAndEnterpriseList(long id, int pageIndex, int pageSize)
        {
            StringBuilder json = new StringBuilder();

            List<Student> students = new StudentOperation().Search(d => d.student_id == id && d.student_isDel == false);
            Student s = students.Count >= 1 ? students[0] : new Student();
            List<Experiment> experiments = base.Search(d => d.experiment_isDel == false && d.experiment_class == (int)ExpermentType.Creativity, d => d.experiment_id, pageIndex, pageSize);
            json.Append("{\"total\":\"" + experiments.Count + "\",\"rows\":[");
            foreach (Experiment experiment in experiments)
            {
                json.Append("{\"id\":\"" + experiment.experiment_id + "\",\"title\":\"" + experiment.experiment_title + "\",\"grade\":\"" + experiment.experiment_allowGrades + "\",\"chooseNum\":\"" + experiment.Chooses.Count(d => d.choose_ownExperimentId == experiment.experiment_id) + "\",\"totalNum\":\"" + experiment.experiment_peopleNum + "\",\"oper\":\"" + GetStudentOperatioon(experiment.experiment_peopleNum, s, experiment.Chooses.Count(d => d.choose_isDel == false), experiment.experiment_id, experiment.experiment_allowGrades.Split(','), false) + "\"},");
            }
            string res = json.ToString();
            res = res.Length >= 2 ? res.Substring(0, res.Length - 1) + "]}" : res;
            return res;
        }

        public bool CheckGrade(string id, long sid)
        {
            Student stu = new StudentOperation().Search(u => u.student_id == sid)[0];
            int grade = stu.student_grade;
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            long exid = Convert.ToInt64(id);
            List<Experiment> es = Search(u => u.experiment_id == exid);
            if (es.Count == 1)
            {

                string[] s = es[0].experiment_allowGrades.Split(',');
                if (s.Contains(grade.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        public string JsonExperiment(string pageIndex, string pageSize, int type)
        {
            int index = 1;
            int size = 30;
            if (string.IsNullOrEmpty(pageIndex) || string.IsNullOrEmpty(pageSize))
            {
                index = 1;
                size = 30;
            }
            index = Convert.ToInt32(pageIndex);
            size = Convert.ToInt32(pageSize);
            long count = SearchCount(u => u.experiment_class == type && u.experiment_isDel == false);

            List<Experiment> experiments = Search(u => u.experiment_isDel == false && u.experiment_class == type, k => k.experiment_id, index, size);

            return ExListToJson(experiments, count, type);
        }


        private string ExListToJson(List<Experiment> experiments, long totalCount, int type)
        {
            StringBuilder sb = new StringBuilder();
            string s = "{\"total\":" + totalCount + ",\"rows\":[";
            sb.Append(s);
            foreach (Experiment e in experiments)
            {
                long id = e.experiment_id;
                long count = new ChooseOperation().SearchCount(u => u.choose_ownExperimentId == id && u.choose_isDel == false);
                s = "{\"id\":\"" + e.experiment_id + "\",\"name\":\"" + e.experiment_title + "\",\"week\":\""+e.experiment_week+"\",\"allGrade\":\"" + e.experiment_allowGrades + "\",\"number\":\"" + count + "\",\"max\":\"" + e.experiment_peopleNum + "\"},";
                sb.Append(s);
            }
            if (experiments.Count != 0)
            {
                s = sb.ToString();
                s = s.Substring(0, s.Length - 1);
            }
            return s + "]}";
        }
        public string RemoveExperiment(string id)
        {
            long exid = Convert.ToInt64(id);
            Experiment e = Search(u => u.experiment_id == exid)[0];
            e.experiment_isDel = true;
            Modify(e, "experiment_isDel");
            return "ok";
        }
        public string AddExperiment(string name, string allowGrade, int type, int maxNum,string week)
        {
            Experiment e = new Experiment();
            e.experiment_isDel = false;
            e.experiment_allowGrades = allowGrade;
            e.experiment_class = type;
            e.experiment_title = name;
            e.experiment_peopleNum = maxNum;
            e.experiment_week = week;
            try
            {
                Add(e);
            }
            catch
            {
                return "添加出错，请检查数据！";

            }
            return "ok";
        }
        public string EditExperiment(string id, string name, string allowGrade, string max,string week)
        {
            int maxNum = Convert.ToInt32(max);
            long exid = Convert.ToInt32(id);
            Experiment e = Search(u => u.experiment_id == exid)[0];
            e.experiment_title = name;
            e.experiment_allowGrades = allowGrade;
            e.experiment_peopleNum = maxNum;
            e.experiment_week = week;
            try
            {
                Modify(e, "experiment_title", "experiment_allowGrades", "experiment_peopleNum", "experiment_week");
            }
            catch
            {
                return "修改出错，请检查数据重试！";
            }
            return "ok";
        }

        public string GetStudentListFromSeniorProject(string id, int page, int rows)
        {
            string res = String.Empty;
            try
            {
                long eId = Convert.ToInt64(id);
                StringBuilder json = new StringBuilder();
                List<Experiment> experiments = base.Search(d => d.experiment_isDel == false && d.experiment_id == eId);
                if (experiments.Count >= 1)
                {
                    List<Choos> chooses = experiments[0].Chooses.Where(d => d.choose_isDel == false).Skip((page - 1) * page).Take(rows).ToList();
                    List<Student> students = new List<Student>();
                    if (chooses.Count >= 1)
                    {
                        foreach (Choos c in chooses)
                        {
                            students.Add(c.Student);
                        }
                    }
                    json.Append("{\"total\":\"" + students.Count(d => d.student_isDel == false) + "\",\"rows\":[");
                    foreach (Student s in students)
                    {
                        string proDir = s.student_proDirectionId == null ? "" : s.Professional.professional_name;
                        json.Append("{\"id\":\"" + s.student_id + "\",\"student_num\":\"" + s.student_number + "\",\"name\":\"" + s.student_name + "\",\"grade\":\"" + s.student_grade + "\",\"professional\":\"" + s.student_professional + "\",\"class\":\"" + s.student_class + "\",\"proDir\":\"" + proDir + "\"},");
                    }
                    res = json.ToString();
                    res = students.Count == 0 ? res + "]}" : res.Substring(0, res.Length - 1) + "]}";
                }
            }
            catch
            {
            }
            return res;
        }

        public JsonStatus OutStudentFromProject(long id, string path)
        {
            StringBuilder json = new StringBuilder();
            List<Experiment> experiments = base.Search(d => d.experiment_isDel == false && d.experiment_id == id);
            if (experiments.Count >= 1)
            {
                List<Choos> chooses = experiments[0].Chooses.Where(d => d.choose_isDel == false).ToList();
                List<Student> students = new List<Student>();
                if (chooses.Count >= 1)
                {
                    foreach (Choos c in chooses)
                    {
                        students.Add(c.Student);
                    }
                }
                return DataProcessing.OutExecl(path, new StudentOperation().ListToDataTable(students), new string[] { "学号", "姓名", "年级", "专业", "班级", "专业方向" }, experiments[0].experiment_title + "学生名单");
            }
            return new JsonStatus() { status = "0", msg = "导出失败！数据异常！" };
        }

        public JsonStatus AddProjectFromSenior(string id, string title, string grade, string totalNum)
        {
            JsonStatus js = new JsonStatus();
            if (title.Length <= 25)
            {
                try
                {
                    int tot = Convert.ToInt32(totalNum);
                    string[] g = grade.Split(',');
                    bool isGrade = true;
                    foreach (string ge in g)
                    {
                        if (!(ge == "1" || ge == "2" || ge == "3" || ge == "4"))
                        {
                            isGrade = false;
                            break;
                        }
                    }
                    if (isGrade)
                    {
                        Experiment e = new Experiment()
                        {
                            experiment_id = 1,
                            experiment_isDel = false,
                            experiment_class = (int)ExpermentType.Graduate,
                            experiment_allowGrades = grade,
                            experiment_peopleNum = tot,
                            experiment_title = title,
                        };
                        base.Add(e);
                        js.status = "1";
                        js.msg = "添加成功！";
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "添加失败！可选年级为1、2、3、4四个选项，多个年级请用“,”分隔！";
                    }
                }
                catch
                {
                    js.status = "0";
                    js.msg = "添加失败！最大人数应为整数！";
                }
            }
            else
            {
                js.status = "0";
                js.msg = "添加失败！课题名称应小于25个汉字！";
            }
            return js;
        }

        public JsonStatus EditProjectFromSenior(string id, string title, string grade, string totalNum)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                long eId = Convert.ToInt64(id);
                if (title.Length <= 25)
                {
                    try
                    {
                        int tot = Convert.ToInt32(totalNum);
                        string[] g = grade.Split(',');
                        bool isGrade = true;
                        foreach (string ge in g)
                        {
                            if (!(ge == "1" || ge == "2" || ge == "3" || ge == "4"))
                            {
                                isGrade = false;
                                break;
                            }
                        }
                        if (isGrade)
                        {
                            try
                            {
                                Experiment e = base.Search(d => d.experiment_isDel == false && d.experiment_id == eId)[0];
                                e.experiment_peopleNum = tot;
                                e.experiment_title = title;
                                e.experiment_allowGrades=grade;
                                base.Modify(e, new string[] { "experiment_peopleNum", "experiment_title", "experiment_allowGrades" });
                                js.status = "1";
                                js.msg = "编辑成功！";
                            }
                            catch
                            {
                                js.status = "0";
                                js.msg = "编辑失败！数据异常！";
                            }
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "编辑失败！可选年级为1、2、3、4四个选项，多个年级请用“,”分隔！";
                        }
                    }
                    catch
                    {
                        js.status = "0";
                        js.msg = "编辑失败！最大人数应为整数！";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "编辑失败！课题名称应小于25个汉字！";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "编辑失败！数据异常！";
            }
            return js;
        }

        public JsonStatus RemoveProjectFromSenior(string id)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                long eId = Convert.ToInt64(id);
                Experiment e = base.Search(d => d.experiment_isDel == false && d.experiment_id == eId)[0];
                e.experiment_isDel = true;
                base.Modify(e, "experiment_isDel");
                js.status = "1";
                js.msg = "删除成功！";
            }
            catch
            {
                js.status = "0";
                js.msg = "删除失败！数据异常！";
            }

            return js;
        }
    }
}