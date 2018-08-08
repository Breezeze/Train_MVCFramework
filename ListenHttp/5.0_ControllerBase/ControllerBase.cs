using ListenHttp;
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

        protected ListenHttpRequest Request { get; }
        protected ListenHttpResponse response { get; }
        /// <summary>
        /// C-V传值字典
        /// </summary>
        protected ViewDataBase ViewData { get; }

        public ControllerBase(ListenHttpRequest request)
        {
            Request = request;
            response = new ListenHttpResponse();
            response.StatusCode = 200;
            response.ContentType = "text/html";
            ViewData = new ViewDataBase();
        }

        #region ActionResult View()     返回最终结果

        /// <summary>
        /// 返回视图
        /// </summary>
        protected ViewResponse View()
        {

            string viewUrl = @"..\..\..\Web" + "\\View\\" + Request.UrlResult.Controller + "\\" + Request.UrlResult.Action + ".html";
            return View(viewUrl);
        }
        protected ViewResponse View(string viewUrl)
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
        protected ViewResponse View(string HtmlStr, bool isHmtlStr)
        {
            if (isHmtlStr)
            {
                response.ResponseString += ViewData.ReplaceViewDataDic(HtmlStr);
                return new ViewResponse(response);
            }
            else
            {
                throw new WebException(500, "服务器内部编码错误");
            }
        }

        #endregion 

    }

}
