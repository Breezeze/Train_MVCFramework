using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ListenHttp
{
    public abstract class ControllerBase
    {
        public ControllerBase(HttpListenerContext _context, UrlResult ur)
        {
            this.context = _context;
            this.urlResult = ur;

            //区分Get-Post请求
            //list为null，是Get请求
            HttpListenerPostParaHelper httppost = new HttpListenerPostParaHelper(context);
            List<HttpListenerPostValue> list = httppost.GetHttpListenerPostValue();
            isGet = list == null;
        }
        protected HttpListenerContext context;
        protected bool isGet;
        protected HttpListenerRequest request { get { return context.Request; } }
        protected HttpListenerResponse response { get { return context.Response; } }
        protected UrlResult urlResult;
        private const string rootDirectory = @"..\..\..\..\Web\View\";

        /// <summary>
        /// C-V传值字典
        /// </summary>
        protected ViewDataBase ViewData = new ViewDataBase();

        /// <summary>
        /// 返回视图
        /// </summary>
        /// <returns></returns>
        protected ActionResult View()
        {
            string viewUrl = rootDirectory + urlResult.Controller + "\\" + urlResult.Action + ".html";
            return View(viewUrl);
        }
        protected ActionResult View(string viewUrl)
        {
            StreamReader sr = new StreamReader(viewUrl, Encoding.UTF8);
            string strhtml = sr.ReadToEnd();

            ////匹配@ViewData["****"]型字符串，***可以为数字字母下划线，不知道为什么不行
            //string regularExpression = "^@ViewData\\[\"[a-zA-Z0-9_]+\"\\]$";

            //实现视图ViewData转为数据
            string regularExpression = "@ViewData\\[\"";
            MatchCollection mc = Regex.Matches(strhtml, regularExpression, RegexOptions.ECMAScript);
            string[] dataName = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)     //matchcollection.Count所匹配项的数量
            {
                int length = strhtml.Substring(mc[i].Index).IndexOf("\"]") - regularExpression.Length;
                dataName[i] = strhtml.Substring(mc[i].Index + regularExpression.Length - 1, length + 1);
            }
            for (int i = 0; i < mc.Count; i++)
            {
                if (ViewData[dataName[i]] != null)
                {
                    strhtml = strhtml.Replace("@ViewData[\"" + dataName[i] + "\"]", (string)ViewData[dataName[i]]);
                }
            }
            return new ActionResult(context.Response, strhtml, 200, "text/html");
        }
        protected ActionResult View(string HtmlStr, bool isHmtlStr)
        {
            if (isHmtlStr)
            {
                ////匹配@ViewData["****"]型字符串，***可以为数字字母下划线，不知道为什么不行
                //string regularExpression = "^@ViewData\\[\"[a-zA-Z0-9_]+\"\\]$";

                //实现视图ViewData转为数据
                string regularExpression = "@ViewData\\[\"";
                MatchCollection mc = Regex.Matches(HtmlStr, regularExpression, RegexOptions.ECMAScript);
                string[] dataName = new string[mc.Count];
                for (int i = 0; i < mc.Count; i++)     //matchcollection.Count所匹配项的数量
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
                return new ActionResult(context.Response, HtmlStr, 200, "text/html");
            }
            else
            {
                throw new WebException(500, context, "服务器内部编码错误！");
            }
        }




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
        }
    }

}
