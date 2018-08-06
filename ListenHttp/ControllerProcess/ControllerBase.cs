using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ListenHttp
{
    public abstract class ControllerBase
    {
        //ControllerBase：提供最基本、核心的行为，与基层服务通讯
        //Controller:提供中层实现，更便利更全面的方法和属性

        public ControllerBase(HttpListenerContext _context, UrlResult ur)
        {
            this.context = _context;
            this.urlResult = ur;
            Request = context.Request;
            Response = context.Response;
        }
        protected HttpListenerContext context;
        protected UrlResult urlResult;
        protected HttpListenerRequest Request;
        protected HttpListenerResponse Response;


        /// <summary>
        /// C-V传值字典
        /// </summary>
        protected ViewDataBase ViewData = new ViewDataBase();


        #region ActionResult View()     返回最终结果

        /// <summary>
        /// 返回视图
        /// </summary>
        protected ActionResult View()
        {

            string viewUrl = Listener.WebRootDirectory + "\\View\\" + urlResult.Controller + "\\" + urlResult.Action + ".html";
            return View(viewUrl);
        }
        protected ActionResult View(string viewUrl)
        {
            StreamReader sr = new StreamReader(viewUrl, Encoding.UTF8);
            string strhtml = sr.ReadToEnd();
            sr.Dispose();
            return View(strhtml, true);
        }
        /// <summary>
        /// 不用视图文件，直接在Controller中编写html代码
        /// </summary>
        /// <param name="isHmtlStr">true</param>
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
                return new ActionResult(HtmlStr, 200, "text/html");
            }
            else
            {
                throw new WebException(500, "服务器内部编码错误！");
            }
        }

        #endregion 

    }

}
