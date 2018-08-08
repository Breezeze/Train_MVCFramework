using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// C-V传值----ViewData
    /// </summary>
    public class ViewDataBase
    {
        private Dictionary<string, string> ViewData = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                return ViewData.ContainsKey(name) ? ViewData[name] : null;
            }
            set
            {
                ViewData.Add(name, value);
            }
        }

        public void Add(string key, string value)
        {
            ViewData.Add(key, value);
        }

        internal string ReplaceViewDataDic(string HtmlStr)
        {

            ////匹配@ViewData["****"]型字符串，***可以为数字字母下划线，不知道为什么不行
            //string regularExpression = "^@ViewData\\[\"[a-zA-Z0-9_]+\"\\]$";
            
            //实现视图ViewData转为数据
            string regularExpression = "@ViewData\\[\"";
            MatchCollection mc = Regex.Matches(HtmlStr, regularExpression, RegexOptions.ECMAScript);
            string[] dataName = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)
            {
                int length = HtmlStr.Substring(mc[i].Index).IndexOf("\"]") - regularExpression.Length;
                dataName[i] = HtmlStr.Substring(mc[i].Index + regularExpression.Length - 1, length + 1);
            }
            for (int i = 0; i < mc.Count; i++)
            {
                if (ViewData[dataName[i]] != null)
                {
                    HtmlStr = HtmlStr.Replace("@ViewData[\"" + dataName[i] + "\"]", ViewData[dataName[i]]);
                }
            }
            return HtmlStr;
        }
    }
}
