using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal class WebException : Exception
    {
        private WebExceptionResponse executeResponse;

        internal WebException(int statusCode, string errorString)
        {
            executeResponse = new ListenHttp.WebExceptionResponse(statusCode, errorString);
        }

        /// <summary>
        /// 外界调用的错误处理程序
        /// </summary>
        /// <param name="ex"></param>
        internal static void ErrorProcess(Exception ex, HttpListenerResponse response)
        {
            if (ex is WebException)
            {
                ((WebException)ex).executeResponse.ManageResponse(response);
            }
            else
            {
                Console.WriteLine("未处理的错误！\n" + ex.Message);
                WebException.ErrorProcess(new WebException(500, "服务器内部错误！"), response);
            }
        }

    }
}
