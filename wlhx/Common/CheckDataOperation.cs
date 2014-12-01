using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class CheckDataOperation
    {
        public static bool CheckData(string str, string regular)
        {
            Regex regex = new Regex(regular);
            Match match = regex.Match(str, 0, str.Length);
            if (match.Length <= 0)
            {
                return false;
            }
            else
            {
                return match.Value == str && match.Length == str.Length;
            }

        }
    }
}
