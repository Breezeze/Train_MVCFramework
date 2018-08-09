using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal static class RequestProcess
    {
        /// <summary>
        /// 根据解析后的url提供不同的请求处理类
        /// </summary>
        internal static IManageMessageRequest ExecuteProcess(UrlResult urlResult)
        {
            if (urlResult["filepath"] != null)
            {
                return new FileRequest();
            }
            else if (urlResult.Controller != null)
            {
                return new ViewRequest();
            }
            else
            {
                throw new WebException(400, "请检测路径的正确性！");
            }
        }
    }
}
