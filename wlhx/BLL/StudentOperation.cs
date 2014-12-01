using BLL;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using wlhx.Common;
using wlhx.Models;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

namespace wlhx.BLL
{
    public class StudentOperation : BaseBLL<Student>
    {
        public Student GetStudent(long id)
        {
            List<Student> stus = Search(m => m.student_id == id && m.student_isDel == false);
            if (stus.Count == 1)
            {
                return stus[0];
            }
            return null;
        }

        public JsonStatus ProfessionChoose(string professionId, long studentId)
        {
            JsonStatus js = new JsonStatus();
            List<Student> students = base.Search(m => m.student_id == studentId && m.student_isDel == false);
            long pId = 0;
            try
            {
                pId = Convert.ToInt64(professionId);

                List<Professional> professional = new ProfessionalOperation().Search(d => d.professional_isDel == false && d.professionalDir_id == pId);
                if (students.Count >= 1 && professional.Count >= 1)
                {
                    if (students[0].student_grade >= 2)
                    {
                        if (students[0].student_proDirectionId==null)
                        {
                            try
                            {
                                students[0].student_proDirectionId = pId;
                                base.Modify(students[0], "student_proDirectionId");
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
        public string Exjson(string pageIndex, string pageSize, int type)
        {
            if (string.IsNullOrEmpty(pageIndex) || string.IsNullOrEmpty(pageSize))
            {
                pageIndex = "1";
                pageSize = "60";
            }
            int ipageIndex = Convert.ToInt32(pageIndex);
            int ipageSize = Convert.ToInt32(pageSize);
            ExperimentOperation eo = new ExperimentOperation();
            long count = eo.SearchCount(u => u.experiment_isDel == false && u.experiment_class == type);
            List<Experiment> experiments = eo.Search(u => u.experiment_isDel == false && u.experiment_class == type, i => i.experiment_title, ipageIndex, ipageSize);
            StringBuilder sb = new StringBuilder();
            string s = "{\"total\":" + count + ",\"rows\":[";
            sb.Append(s);
            foreach (Experiment e in experiments)
            {
                long id = e.experiment_id;
                long chooseSNum = new ChooseOperation().SearchCount(u => u.choose_id == id && u.choose_isDel == false);
                s = "{\"id\":\"" + e.experiment_id + "\",\"title\":\"" + e.experiment_title + "\",\"num\":\"" + chooseSNum + "\",\"max\":\"" + e.experiment_peopleNum + "\",\"btn\":\" <button class='btn' value='" + e.experiment_id + "' type='button'>选择</button>\"},";
                sb.Append(s);
            }
            s = sb.ToString();
            if (count > 0)
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s + "]}";
        }

        public string MyProject(string type, long studentId)
        {
            int t = 0;
            StringBuilder json = new StringBuilder();
            string res = String.Empty;
            try
            {
                t = Convert.ToInt32(type);
                Student s = base.Search(d => d.student_id == studentId && d.student_isDel == false)[0];
                List<Choos> chooses = s.Chooses.Where(d => d.choose_isDel == false && d.Experiment.experiment_class == t).ToList();
                json.Append("{\"total\":\"" + chooses.Count() + "\",\"rows\":[");
                foreach (Choos choos in chooses)
                {
                    json.Append("{\"id\":\"" + choos.choose_id + "\",\"title\":\"" + choos.Experiment.experiment_title + "\",\"num\":\"" + GetChooesPeopelNum(choos.choose_ownExperimentId) + "\"},");
                }
                res = json.ToString();
                res = res.Length >= 2 ? res.Substring(0, res.Length - 1) + "]}" : res;
                return res;

            }
            catch
            {

            }
            return res;
        }

        private string GetChooesPeopelNum(long id)
        {
            return "已选" + new ChooseOperation().SearchCount(d => d.choose_ownExperimentId == id && d.choose_isDel == false) + "人";
        }

        public string GetPeopleMsg(long id)
        {
            string res = String.Empty;
            try
            {
                StringBuilder json = new StringBuilder();
                json.Append("[");
                Student s = base.Search(d => d.student_id == id && d.student_isDel == false)[0];
                string professional = s.Professional == null ? null : s.Professional.professional_name;
                json.Append("{\"name\":\"学号\",\"value\":\"" + s.student_number + "\",\"group\":\"学号\"},");
                json.Append("{\"name\":\"姓名\",\"value\":\"" + s.student_name + "\",\"group\":\"姓名\"},");
                json.Append("{\"name\":\"年级\",\"value\":\"" + s.student_grade + "\",\"group\":\"年级\"},");
                json.Append("{\"name\":\"专业\",\"value\":\"" + s.student_professional + "\",\"group\":\"专业\"},");
                json.Append("{\"name\":\"班级\",\"value\":\"" + s.student_class + "\",\"group\":\"班级\"},");
                json.Append("{\"name\":\"专业方向\",\"value\":\"" + professional + "\",\"group\":\"专业方向\"},");
                res = json.ToString();
                res = res.Substring(0, res.Length - 1) + "]";
            }
            catch
            { }
            return res;
        }
        public bool CheckGrade(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            long exid = Convert.ToInt64(id);
            return false;
        }

        public JsonStatus RePassword(string jpwd, string xpwd, string qpwd, long id)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                Student s = base.Search(d => d.student_id == id && d.student_isDel == false)[0];
                if (CheckDataOperation.CheckData(xpwd, @"^[A-Za-z0-9]{1,10}$"))
                {
                    if (xpwd == qpwd)
                    {
                        if (s.student_password == Md5.GetMd5Word(jpwd, s.student_number))
                        {
                            s.student_password = Md5.GetMd5Word(xpwd, s.student_number);
                            base.Modify(s, "student_password");
                            js.status = "0";
                            js.msg = "修改成功！";
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "修改失败！旧密码错误.....";
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "修改失败！二次输入的密码不一致.....";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "修改失败！数据不合法.....";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "修改失败！数据异常.....";
            }
            return js;

        }

        public string GetStudentList(int pageIndex, int pageSize, int grade, int professional)
        {
            StringBuilder json = new StringBuilder();
            List<int> gradeList = grade == 0 ? new List<int> { 1, 2, 3, 4 } : new List<int> { grade };
            List<long> professionalList = professional == 0 ? new ProfessionalOperation().Search(d => d.professional_isDel == false).Select(d => d.professionalDir_id).ToList() : new List<long>() { professional };
            json.Append("{\"total\":\"" + base.SearchCount(d => d.student_isDel == false && gradeList.Contains(d.student_grade)) + "\",\"rows\":[");
            List<Student> students = base.Search(d => d.student_isDel == false && gradeList.Contains(d.student_grade) && (professionalList.Contains((long)d.student_proDirectionId) || (d.student_proDirectionId == null && professional == 0)), d => d.student_id, pageIndex, pageSize);
            foreach (Student s in students)
            {
                string proDir = s.student_proDirectionId == null ? "" : s.Professional.professional_name;
                json.Append("{\"id\":\"" + s.student_id + "\",\"student_num\":\"" + s.student_number + "\",\"name\":\"" + s.student_name + "\",\"grade\":\"" + s.student_grade + "\",\"professional\":\"" + s.student_professional + "\",\"class\":\"" + s.student_class + "\",\"proDir\":\"" + proDir + "\"},");
            }
            string res = json.ToString();
            res = students.Count == 0 ? res + "]}" : res.Substring(0, res.Length - 1) + "]}";
            return res;
        }

        public JsonStatus InputStudent(string path, HttpPostedFileBase file)
        {
            JsonStatus js = new JsonStatus();
            string filename = file.FileName;
            SaveFile sf = new SaveFile();
            if (sf.FileCheck(filename))
            {
                filename = sf.SaveFileTo(path, file);
                if (filename != "标题或文件不能为空.....")
                {
                    DataTable dt = sf.OpenFile(path + "/" + filename.Split('/')[filename.Split('/').Length - 1]);
                    if (dt.Rows.Count != 0)
                    {
                        js = this.DataCheck(dt);
                        if (js.status == "1")
                        {
                            js = this.WriteStudentData(dt);
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "导入失败！文件格式错误或损坏！";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "导入失败！文件为空！";
                }

            }
            else
            {
                js.status = "0";
                js.msg = "只支持后缀为xlsx、xls的Excel文件！";
            }
            return js;
        }

        private JsonStatus DataCheck(DataTable dt)
        {
            JsonStatus js = new JsonStatus();
            List<string> student_num = base.Search(d => d.student_isDel == false).Select(d => d.student_number).ToList();
            foreach (DataRow data in dt.Rows)
            {
                if (!student_num.Contains(data[0].ToString()))
                {
                    if (CheckDataOperation.CheckData(data[0].ToString(), @"^[0-9]{1,11}$"))
                    {
                        if (data[2].ToString() == "1" || data[2].ToString() == "2" || data[2].ToString() == "3" || data[2].ToString() == "4")
                        {
                            if (!String.IsNullOrEmpty(data[1].ToString()) && data[1].ToString().Length <= 25)
                            {
                                if (!String.IsNullOrEmpty(data[3].ToString()) && data[3].ToString().Length <= 25)
                                {
                                    if (!String.IsNullOrEmpty(data[4].ToString()) && data[4].ToString().Length <= 25)
                                    {
                                        student_num.Add(data[0].ToString());
                                        js.status = "1";
                                        js.msg = "数据正确！";
                                    }
                                    else
                                    {
                                        js.status = "0";
                                        js.msg = "导入失败！学生班级不能为空并且应小于25个汉字！";
                                        break;
                                    }
                                }
                                else
                                {
                                    js.status = "0";
                                    js.msg = "导入失败！学生专业不能为空并且应小于25个汉字！";
                                    break;
                                }
                            }
                            else
                            {
                                js.status = "0";
                                js.msg = "导入失败！学生姓名不能为空并且应小于25个汉字！";
                                break;
                            }
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "导入失败！学生年级错误！年级只能是1、2、3、4四个选项！";
                            break;
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "导入失败！学生学号格式错误！学号只能是1-11位数字！";
                        break;
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "导入失败！学生学号错误,存在存在重复的学号或已经存在的学号！";
                    break;
                }
            }
            return js;
        }

        private JsonStatus WriteStudentData(DataTable dt)
        {
            JsonStatus js = new JsonStatus() { status = "1", msg = "导入成功！" };
            foreach (DataRow data in dt.Rows)
            {
                try
                {
                    Student s = new Student()
                    {
                        student_number = data[0].ToString(),
                        student_name = data[1].ToString(),
                        student_grade = Convert.ToInt32(data[2]),
                        student_professional = data[3].ToString(),
                        student_class = data[4].ToString(),
                        student_isDel = false,
                        student_id = 1,
                        student_password = Md5.GetMd5Word("123456", data[0].ToString())
                    };
                    base.Add(s);
                }
                catch
                {
                    js.status = "0";
                    js.status = "导入完成！但有的项为被导入！";
                }
            }
            return js;
        }

        public JsonStatus AddStudent(string student_Num, string name, string grade, string professional, string _class, string proDir)
        {
            JsonStatus js = new JsonStatus();
            List<string> student_num = base.Search(d => d.student_isDel == false).Select(d => d.student_number).ToList();
            List<long> pd = new ProfessionalOperation().Search(d => d.professional_isDel == false).Select(d => d.professionalDir_id).ToList();
            try
            {
                long pId = proDir == "" ? -1 : Convert.ToInt64(proDir);
                if (pId == -1 || pd.Contains(pId))
                {
                    if (!student_num.Contains(student_Num))
                    {
                        if (CheckDataOperation.CheckData(student_Num, @"^[0-9]{1,11}$"))
                        {
                            if (grade == "1" || grade == "2" || grade == "3" || grade == "4")
                            {
                                if (!String.IsNullOrEmpty(name) && name.Length <= 25)
                                {
                                    if (!String.IsNullOrEmpty(professional) && professional.Length <= 25)
                                    {
                                        if (!String.IsNullOrEmpty(_class) && _class.Length <= 25)
                                        {
                                            try
                                            {
                                                Student s = proDir == "" ? new Student()
                                                {
                                                    student_id = 1,
                                                    student_number = student_Num,
                                                    student_name = name,
                                                    student_grade = Convert.ToInt32(grade),
                                                    student_class = _class,
                                                    student_professional = professional,
                                                    student_isDel = false,
                                                    student_password = Md5.GetMd5Word("123456", student_Num)
                                                } : new Student()
                                                {
                                                    student_id = 1,
                                                    student_number = student_Num,
                                                    student_name = name,
                                                    student_grade = Convert.ToInt32(grade),
                                                    student_class = _class,
                                                    student_professional = professional,
                                                    student_proDirectionId = pId,
                                                    student_isDel = false,
                                                    student_password = Md5.GetMd5Word("123456", student_Num)
                                                };
                                                base.Add(s);
                                                js.status = "1";
                                                js.msg = "添加成功！";
                                            }
                                            catch
                                            {
                                                js.status = "0";
                                                js.msg = "添加失败！未知错误！";
                                            }
                                        }
                                        else
                                        {
                                            js.status = "0";
                                            js.msg = "添加失败！学生班级不能为空并且应小于25个汉字！";
                                        }
                                    }
                                    else
                                    {
                                        js.status = "0";
                                        js.msg = "添加失败！学生专业不能为空并且应小于25个汉字！";
                                    }
                                }
                                else
                                {
                                    js.status = "0";
                                    js.msg = "添加失败！学生姓名不能为空并且应小于25个汉字！";
                                }
                            }
                            else
                            {
                                js.status = "0";
                                js.msg = "添加失败！学生年级错误！年级只能是1、2、3、4四个选项！";
                            }
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "添加失败！学生学号格式错误！学号只能是1-11位数字！";
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "添加失败！学生学号错误,存在存在重复的学号或已经存在的学号！";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "添加失败！学生专业方向错误,不存在此专业方向！";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "添加失败！数据错误！";
            }

            return js;
        }

        public JsonStatus EditStudent(string id, string student_Num, string name, string grade, string professional, string _class, string proDir)
        {
            JsonStatus js = new JsonStatus();
            List<string> student_num = base.Search(d => d.student_isDel == false).Select(d => d.student_number).ToList();
            List<Professional> pd = new ProfessionalOperation().Search(d => d.professional_isDel == false);

            try
            {
                long pId = proDir == "" ? -1 : (pd.Select(d => d.professional_name).ToList().Contains(proDir) ? pd.Where(d => d.professional_name == proDir).Select(d => d.professionalDir_id).ToList()[0] : Convert.ToInt64(proDir));
                long studentId = Convert.ToInt64(id);
                if (pd.Select(d => d.professionalDir_id).Contains(pId) || pId == -1)
                {
                    if (student_num.Contains(student_Num))
                    {
                        if (CheckDataOperation.CheckData(student_Num, @"^[0-9]{1,11}$"))
                        {
                            if (grade == "1" || grade == "2" || grade == "3" || grade == "4")
                            {
                                if (!String.IsNullOrEmpty(name) && name.Length <= 25)
                                {
                                    if (!String.IsNullOrEmpty(professional) && professional.Length <= 25)
                                    {
                                        if (!String.IsNullOrEmpty(_class) && _class.Length <= 25)
                                        {
                                            try
                                            {
                                                List<Student> students = base.Search(d => d.student_isDel == false && d.student_id == studentId);
                                                if (students.Count >= 1)
                                                {
                                                    students[0].student_number = student_Num;
                                                    students[0].student_name = name;
                                                    students[0].student_grade = Convert.ToInt32(grade);
                                                    students[0].student_class = _class;
                                                    students[0].student_professional = professional;
                                                    students[0].student_proDirectionId = pId;
                                                    base.Modify(students[0], new string[] { "student_number", "student_name", "student_grade", "student_class", "student_professional", pId == -1 ? "student_number" : "student_proDirectionId" });
                                                    js.status = "1";
                                                    js.msg = "修改成功！";
                                                }
                                                else
                                                {
                                                    js.status = "0";
                                                    js.msg = "修改失败！数据异常！";
                                                }
                                            }
                                            catch
                                            {
                                                js.status = "0";
                                                js.msg = "修改失败！未知错误！";
                                            }
                                        }
                                        else
                                        {
                                            js.status = "0";
                                            js.msg = "修改失败！学生班级不能为空并且应小于25个汉字！";
                                        }
                                    }
                                    else
                                    {
                                        js.status = "0";
                                        js.msg = "修改失败！学生专业不能为空并且应小于25个汉字！";
                                    }
                                }
                                else
                                {
                                    js.status = "0";
                                    js.msg = "修改失败！学生姓名不能为空并且应小于25个汉字！";
                                }
                            }
                            else
                            {
                                js.status = "0";
                                js.msg = "修改失败！学生年级错误！年级只能是1、2、3、4四个选项！";
                            }
                        }
                        else
                        {
                            js.status = "0";
                            js.msg = "修改失败！学生学号格式错误！学号只能是1-11位数字！";
                        }
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "修改失败！学生学号错误,存在存在重复的学号或已经存在的学号！";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "修改失败！学生专业方向错误,不存在此专业方向！";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "修改失败！数据错误！";
            }

            return js;
        }

        public JsonStatus RemoveStudent(string id)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                long studentId = Convert.ToInt64(id);
                List<Student> students = base.Search(d => d.student_id == studentId && d.student_isDel == false);
                if (students.Count >= 1)
                {
                    students[0].student_isDel = true;
                    base.Modify(students[0], "student_isDel");
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

        public JsonStatus ResetStudentPassword(string id)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                long studentId = Convert.ToInt64(id);
                List<Student> students = base.Search(d => d.student_id == studentId && d.student_isDel == false);
                if (students.Count >= 1)
                {
                    students[0].student_password = Md5.GetMd5Word("123456", students[0].student_number);
                    base.Modify(students[0], "student_password");
                    js.status = "1";
                    js.msg = "重置成功！";
                }
                else
                {
                    js.status = "0";
                    js.msg = "重置失败！数据异常！";
                }
            }
            catch
            {
                js.status = "0";
                js.msg = "重置失败！数据异常！";
            }
            return js;
        }

        public string StudentDetailedMsgList(string id)
        {
            string res = String.Empty;
            StringBuilder json = new StringBuilder();
            try
            {
                long studentId = Convert.ToInt64(id);
                json.Append("[");
                Student s = base.Search(d => d.student_id == studentId && d.student_isDel == false)[0];
                List<Choos> chooses = new List<Choos>();
                chooses = s.Chooses.Where(d => d.choose_isDel == false && d.Experiment.experiment_class == (int)ExpermentType.Experment).ToList();
                for (int i = 0; i < chooses.Count; i++)
                {
                    json.Append("{\"id\":\"" + chooses[i].choose_id + "\",\"name\":\"实验" + (i + 1) + "\",\"value\":\"" + chooses[i].Experiment.experiment_title + "\",\"time\":\"" + chooses[i].choose_ExperimenTimet + "\",\"group\":\"实验\",\"oper\":\"<button class='oper' value='" + chooses[i].choose_id + "' >删除</button>\"},");
                }
                chooses = s.Chooses.Where(d => d.Experiment.experiment_class == (int)ExpermentType.Creativity && d.choose_isDel == false).ToList();
                for (int i = 0; i < chooses.Count; i++)
                {
                    json.Append("{\"id\":\"" + chooses[i].choose_id + "\",\"name\":\"课题" + (i + 1) + "\",\"value\":\"" + chooses[i].Experiment.experiment_title + "\",\"time\":\"------\",\"group\":\"创新创业\",\"oper\":\"<button class='oper' value='" + chooses[i].choose_id + "'>删除</button>\"},");
                }
                chooses = s.Chooses.Where(d => d.Experiment.experiment_class == (int)ExpermentType.Graduate && d.choose_isDel == false).ToList();
                for (int i = 0; i < chooses.Count; i++)
                {
                    json.Append("{\"id\":\"" + chooses[i].choose_id + "\",\"name\":\"课题" + (i + 1) + "\",\"value\":\"" + chooses[i].Experiment.experiment_title + "\",\"time\":\"------\",\"group\":\"毕业课题\",\"oper\":\"<button class='oper' value='" + chooses[i].choose_id + "' >删除</button>\"},");
                }
            }
            catch
            { }
            res = json.ToString();
            res = res.Length >= 2 ? res.Substring(0, res.Length - 1) + "]" : res + "]";
            return res;
        }

        public JsonStatus ModifyStudentFromProfessional(string mark, string student_num, long professionalId)
        {
            JsonStatus js = new JsonStatus();
            List<Student> students = base.Search(d => d.student_number == student_num && d.student_isDel == false);
            if (students.Count >= 1)
            {
                if ((students[0].student_proDirectionId == null && mark == "add") || (mark == "remove"))
                {
                    students[0].student_proDirectionId = mark == "add" ? professionalId : (long?)null;
                    base.Modify(students[0], "student_proDirectionId");
                    js.status = "1";
                    js.msg = mark == "add" ? "添加成功！" : "删除成功！";
                }
                else
                {
                    js.status = "0";
                    js.msg = "该学生已有一个专业方向！请先将其删除后添加！";
                }
            }
            else
            {
                js.status = "0";
                js.msg = mark == "add" ? "输入的学号不存在！请检查输入是否正确！" : "数据异常！";
            }
            return js;
        }

        public JsonStatus OutStudent(string path, long proDirId)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                List<long> proDirIdList = proDirId == 0 ? new ProfessionalOperation().Search(d => d.professional_isDel == false).Select(d => d.professionalDir_id).ToList() : new List<long>() { proDirId };
                List<Student> students = base.Search(d => d.student_isDel == false && (proDirIdList.Contains((long)d.student_proDirectionId) || (proDirId == 0 && d.student_proDirectionId == null)));
                js = DataProcessing.OutExecl(path, this.ListToDataTable(students), new string[] { "学号", "姓名", "年级", "专业", "班级", "专业方向" }, "学生名单");
            }
            catch
            {
                js.status = "0";
                js.msg = "导出失败！未知错误！";
            }
            return js;
        }

        public DataTable ListToDataTable(List<Student> students)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("学号", typeof(string));
            DataColumn dc2 = new DataColumn("姓名", typeof(string));
            DataColumn dc3 = new DataColumn("年级", typeof(string));
            DataColumn dc4 = new DataColumn("专业", typeof(string));
            DataColumn dc5 = new DataColumn("班级", typeof(string));
            DataColumn dc6 = new DataColumn("专业方向", typeof(string));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            for (int i = 0; i < students.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = students[i].student_number;
                dr[1] = students[i].student_name;
                dr[2] = students[i].student_grade;
                dr[3] = students[i].student_professional;
                dr[4] = students[i].student_class;
                dr[5] = students[i].student_proDirectionId == null ? "" : students[i].Professional.professional_name;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public string GetStudentJson(string id, bool isExtime, string time)
        {
            long exid = Convert.ToInt64(id);
            List<Choos> chos = new List<Choos>();
            if (time == "全部")
            {
                chos = new ChooseOperation().Search(i => i.choose_ownExperimentId == exid && i.choose_isDel == false);
            }
            else
            {
                chos = new ChooseOperation().Search(i => i.choose_ownExperimentId == exid && i.choose_isDel == false && i.choose_ExperimenTimet == time);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":" + chos.Count + ",\"rows\":[");
            foreach (Choos c in chos)
            {
                sb.Append("{\"id\":\"" + c.Student.student_number + "\",\"name\":\"" + c.Student.student_name + "\",\"pro\":\"" + c.Student.student_professional + "\",\"grade\":\"" + c.Student.student_grade + "\",\"dir\":\"" + c.Student.Professional.professional_name + "\",\"time\":\"" + c.choose_ExperimenTimet + "\"},");
            }
            string s = sb.ToString();

            if (chos.Count != 0)
            {
                s = s.Substring(0, s.Length - 1);
            }

            return s + "]}";
        }
        public string AddChooseStu(string id, string exid)
        {
            try
            {
                List<Student> stus = Search(u => u.student_number == id && u.student_isDel == false);
                if (stus.Count == 0)
                {
                    return "没有此学号的学生";
                }
                else if (stus.Count > 1)
                {
                    return "学号重复，添加失败";
                }
                ChooseOperation co = new ChooseOperation();
                long stuid = stus[0].student_id;
                long experId = Convert.ToInt64(exid);
                long count = co.SearchCount(u => u.choose_ownStudentId == stuid && u.choose_isDel == false && u.choose_ownExperimentId == experId);
                if (count > 0)
                {
                    return "此同学已经存在，请勿重新添加";
                }
                Choos c = new Choos();
                c.choose_isDel = false;
                c.choose_ownExperimentId = Convert.ToInt64(exid);
                c.choose_ownStudentId = stus[0].student_id;

                co.Add(c);
            }
            catch
            {
                return "未知错误";
            }
            return "ok";
        }
        public string AddExperimentChooseStu(string id, string exid, string time)
        {
            try
            {
                List<Student> stus = Search(u => u.student_number == id && u.student_isDel == false);
                if (stus.Count == 0)
                {
                    return "没有此学号的学生";
                }
                else if (stus.Count > 1)
                {
                    return "学号重复，添加失败";
                }
                ChooseOperation co = new ChooseOperation();
                long stuid = stus[0].student_id;
                long experId = Convert.ToInt64(exid);
                long count = co.SearchCount(u => u.choose_ownStudentId == stuid && u.choose_isDel == false);
                if (count > 0)
                {
                    return "此同学已经存在，请勿重新添加";
                }
                ExperimentTime et = new ExperimentTimeOperation().Search(u => u.experimentTime_id == experId)[0];

                Choos c = new Choos();
                c.choose_isDel = false;
                c.choose_ownExperimentId = et.Experiment.experiment_id;
                c.choose_ownStudentId = stus[0].student_id;
                c.choose_ExperimenTimet = time;
                co.Add(c);
            }
            catch
            {
                return "未知错误";
            }
            return "ok";
        }
        public string DelChoose(string id, string exid)
        {
            List<Student> stus = Search(u => u.student_number == id && u.student_isDel == false);
            if (stus.Count == 0)
            {
                return "没有此学号的学生";
            }
            long sid = stus[0].student_id;
            long experimentid = Convert.ToInt64(exid);
            List<Choos> cs = new ChooseOperation().Search(u => u.choose_ownStudentId == sid && u.choose_isDel == false && u.choose_ownExperimentId == experimentid);
            cs[0].choose_isDel = true;
            new ChooseOperation().Modify(cs[0], "choose_isDel");
            return "ok";

        }
        public string DelExtimeChooseStu(string id, string exid)
        {
            List<Student> stus = Search(u => u.student_number == id && u.student_isDel == false);
            if (stus.Count == 0)
            {
                return "没有此学号的学生";
            }
            long sid = stus[0].student_id;
            long extid = Convert.ToInt64(exid);
            List<Choos> cs = new ChooseOperation().Search(u => u.choose_ownStudentId == sid && u.choose_isDel == false);
            cs[0].choose_isDel = true;
            try
            {
                new ChooseOperation().Modify(cs[0], "choose_isDel");
            }
            catch
            {
                return "未知错误！";
            }
            return "ok";

        }

    }
}