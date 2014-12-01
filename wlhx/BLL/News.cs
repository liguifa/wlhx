using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wlhx.Common;
using wlhx.Models;

namespace wlhx.BLL
{
    public class News : BaseBLL<Dynamic>
    {
        public List<wlhx.Models.Dynamic> GetNewsList(int pageIndex, int pageSize, out long totalNum, int DynamicTypes)
        {
            totalNum = SearchCount(u => u.dynamic_isDel == false && u.dynamic_class == DynamicTypes);
            List<Dynamic> newsList = Search(k => k.dynamic_isDel == false && k.dynamic_class == DynamicTypes, k => k.dynamic_publicTime, pageIndex, pageSize);
            
            return newsList;
        }
    }
}