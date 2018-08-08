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
    public abstract class Controller : ControllerBase
    {
        /// <summary>
        /// 控制器方法列表
        /// </summary>
        protected static List<string[]> ctrlAction { get; set; }

        public Controller(ListenHttpRequest request) : base(request)
        {
            if (ctrlAction == null)
            {
                ctrlAction = RequestProcess.ctrlActions;
            }

        }

        /// <summary>
        /// 请求参数列表
        /// </summary>
        protected string GetRequestForm(string key)
        {
            return Request.Form.ContainsKey(key) ? Request.Form[key] : null;
        }

        /// <summary>
        /// 写入响应流数据
        /// </summary>
        protected void ResponseWrite(string responseString)
        {
            response.ResponseString += responseString;
        }

    }
}
