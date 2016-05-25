using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public static class Helper
    {
        public static string ToDisplay(this List<string> list, string separator = ", ")
        {
            if (list.Count == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                sb.Append(string.Format("{0}{1}", separator, list[i]));
            }
            return sb.ToString();
        }

        public static string ToDisplay(this List<string> list, string exclude, string separator = ", ")
        {
           return list.ToDisplay().Replace(string.Format("{0}{1}", separator, exclude), "").Replace(string.Format("{0}{1}", exclude, separator.Trim()), "").Trim();
        }

        public static string ToPercentString(this object item)
        {
            return item.ToString() + " %";
        }
    }
}
