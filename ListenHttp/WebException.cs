using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class WebException : Exception
    {
        public WebException(int statusCode)
        {
            this.statusCode = statusCode;
        }
        public WebException(int statusCode, HttpListenerContext context, string errorString)
        {
            this.statusCode = statusCode;
            this.errorString = errorString;
            this.context = context;
        }
        private int statusCode;
        private string errorString;
        private HttpListenerContext context;

        /// <summary>
        /// 发送错误信息
        /// </summary>
        private void SendErrorResponse()
        {
            HttpListenerResponse response = context.Response;
            response.StatusCode = statusCode;
            response.ContentEncoding = Encoding.UTF8;
            string responseString = "<html><head><meta charset='utf-8'></head><body>请求失败！<br />" + errorString + "</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        /// <summary>
        /// 外界调用的错误处理程序
        /// </summary>
        /// <param name="ex"></param>
        public static void ErrorProcess(Exception ex, HttpListenerContext context)
        {
            if (ex is WebException)
            {
                ((WebException)ex).SendErrorResponse();
            }
            else
            {
                if (ex.InnerException is WebException)
                {
                    WebException webexc = ex.InnerException as WebException;
                    (new WebException(webexc.statusCode, context, ex.Message)).SendErrorResponse();
                }
                else
                {
                    (new WebException(500, context, "服务器内部错误！")).SendErrorResponse();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
