using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wlhx.Common
{
    public class Html
    {
        public static String strToHtml(String s)
        {
         
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("  ", " ");
            s = s.Replace("\r\n", "<br/>");
            s = s.Replace("\n", "");
            s = s.Replace("\t", "");
            s = s.Replace("/n", "<br/>");
            s = s.Replace("'", "'");    
            return s;
        }   
    }
}