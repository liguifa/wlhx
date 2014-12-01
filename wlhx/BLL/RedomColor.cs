using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wlhx.BLL
{
    public class RedomColor
    {
        private static readonly string[] s = { "#D30D15", "#3498DB", "#1FA67A", "#E48632", "#F43C12", "#2B9646", "#00AEEF", "#F05033", "#B94A48", "#222222" };
        private static int i = 0;
        public static  string getRedomColor()
        {
            int r = 0;
            r = getRedomInt();
            while (r == i) 
            {
                r = getRedomInt();
            }
            i = r;
            return s[r];
        }
        private static int getRedomInt()
        {
            Random r = new Random();

            return r.Next(10); 
        }
    }
}