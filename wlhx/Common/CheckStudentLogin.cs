using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wlhx.BLL;
using wlhx.Models;

namespace wlhx.Common
{
    public static class CheckStudentLogin
    {
        public static bool Check(string username, string passwd, out long id)
        {
            passwd=Md5.GetMd5Word(passwd, username);
            List<Student> students = new StudentOperation().Search(u => u.student_number == username && u.student_password ==passwd );
            if (students.Count == 1)
            {
                id = students[0].student_id;
                return true;
            }
            id = 0;
            return false;

        }
    }
}