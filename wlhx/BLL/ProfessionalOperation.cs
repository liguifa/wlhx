using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using wlhx.Common;
using wlhx.Models;

namespace wlhx.BLL
{
    public class ProfessionalOperation : BaseBLL<Professional>
    {
        public string GetProfessionaLEmphasis(long id)
        {
            List<Student> students = new StudentOperation().Search(d => d.student_isDel == false && d.student_id == id);
            Student s = students.Count >= 1 ? students[0] : new Student();
            string json = "[";
            List<Professional> professionals = base.Search(d => d.professional_isDel == false);
            foreach (Professional professional in professionals)
            {
                json = json + "{\"id\":\"" + professional.professionalDir_id + "\",\"name\":\"" + professional.professional_name + "\",\"choose\":\"" + this.GetStudentOperation(professional.professional_name, s, professional.professionalDir_id) + "\"},";
            }
            json = json.Substring(0, json.Length - 1) + "]";
            return json;
        }

        private string GetStudentOperation(string value, Student s, long id)
        {
            return s.student_grade >= 2 ? (s.student_proDirectionId == null ? "<button class='choose btn btn-default' value='" + id + "'>选择</button>" : (s.Professional.professional_name == value ? "已选择" : "---")) : "---";
            //if (s.student_grade >= 2)
            //{
            //    if (s.student_professional == null)
            //    {
            //        return "<button class='choose btn btn-default' value='" + id + "'>选择</button>";
            //    }
            //    else
            //    {
            //        if (s.student_professional == value)
            //        {
            //            return "已选择";
            //        }
            //        else
            //        {
            //            return "---";
            //        }
            //    }
            //}
            //else
            //{
            //    return "---";
            //}
        }

        public string GetProfessionalList()
        {
            StringBuilder json = new StringBuilder();
            List<Professional> professionals = base.Search(d => d.professional_isDel == false);
            json.Append("[");
            foreach (Professional professional in professionals)
            {
                json.Append("{\"value\":\"" + professional.professionalDir_id + "\",\"text\":\"" + professional.professional_name + "\",\"people_num\":\"" + professional.Students.Count(d=>d.student_isDel==false) + "\"},");
            }
            string res = json.ToString();
            res = res.Length >= 2 ? res.Substring(0, res.Length - 1) + "]" : res + "]";
            return res;
        }

        public JsonStatus AddProfessional(string title)
        {
            JsonStatus js = new JsonStatus();
            if (base.SearchCount(d => d.professional_name == title && d.professional_isDel == false) < 1)
            {
                if (!String.IsNullOrEmpty(title) && title.Length <= 25)
                {
                    Professional p = new Professional()
                    {
                        professionalDir_id = 1,
                        professional_isDel = false,
                        professional_name = title
                    };
                    base.Add(p);
                    js.status = "1";
                    js.msg = "添加成功！";
                }
                else
                {
                    js.status = "0";
                    js.msg = "方向名称非空并小于25个汉字！";
                }
            }
            else
            {
                js.status = "0";
                js.msg = "此专业方向已存在！不能重复添加！";
            }
            return js;
        }

        public JsonStatus EditProfessional(long id, string title)
        {
            JsonStatus js = new JsonStatus();
            List<Professional> professionals = base.Search(d => d.professional_isDel == false && d.professionalDir_id == id);
            if (professionals.Count >= 1)
            {
                if (base.SearchCount(d => d.professional_name == title && d.professional_isDel == false) < 1)
                {
                    if (!String.IsNullOrEmpty(title) && title.Length <= 25)
                    {
                        professionals[0].professional_name = title;
                        base.Modify(professionals[0], "professional_name");
                        js.status = "1";
                        js.msg = "编辑成功！";
                    }
                    else
                    {
                        js.status = "0";
                        js.msg = "方向名称非空并小于25个汉字！";
                    }
                }
                else
                {
                    js.status = "0";
                    js.msg = "此专业方向已存在！不能编辑为相同的名称！";
                }
            }
            else
            {
                js.status = "0";
                js.msg = "编辑失败！数据异常！";
            }
            return js;
        }

        public JsonStatus RemoveProfessional(long id)
        {
            JsonStatus js = new JsonStatus();
            List<Professional> professionals = base.Search(d => d.professional_isDel == false && d.professionalDir_id == id);
            if (professionals.Count >= 1)
            {
                List<Student> students = professionals[0].Students.Where(d=>d.student_isDel==false).Select(d=>d).ToList();
                StudentOperation so = new StudentOperation();
                foreach (Student s in students)
                {
                    s.student_proDirectionId = null;
                    so.Modify(s, "student_proDirectionId");
                }
                professionals[0].professional_isDel = true;
                base.Modify(professionals[0], "professional_isDel");
                js.status = "1";
                js.msg = "删除成功！";
            }
            else
            {
                js.status = "0";
                js.msg = "删除失败！数据异常！";
            }
            return js;
        }

    }
}