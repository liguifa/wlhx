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
using wlhx.Models;

namespace wlhx.BLL
{
    public class StudentImgOperation : BaseBLL<wlhx.Models.StudentImg>
    {
        public List<StudentImg> GetImg()
        {
            return base.Search(d => d.studentImg_isDel == false);
        }

        public long GetImageWidth()
        {
            long num = base.SearchCount(d => d.studentImg_isDel == false);
            return num * 586;
        }
    }
}