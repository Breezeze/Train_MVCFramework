using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal class WebException : Exception, ISendResponse
    {
        internal WebException(int statusCode, string errorString)
        {
            this.statusCode = statusCode;
            this.errorString = errorString;
        }
        private int statusCode;
        private string errorString;

        /// <summary>
        /// 外界调用的错误处理程序
        /// </summary>
        /// <param name="ex"></param>
        internal static void ErrorProcess(Exception ex, HttpListenerResponse response)
        {
            if (ex is WebException)
            {
                ((WebException)ex).SendResponse(response);
            }
            else
            {
                Console.WriteLine(ex.Message);
                (new WebException(500, "服务器内部错误！")).SendResponse(response);
            }
        }

        public void SendResponse(HttpListenerResponse response)
        {
            string html = "<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\"></head><body>" + "请求失败！\n" + errorString + "</body></html>";
            response.StatusCode = statusCode;
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            byte[] buffer = Encoding.UTF8.GetBytes(html);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
