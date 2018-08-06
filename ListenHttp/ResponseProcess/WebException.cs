using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class WebException : Exception, ISendResponse
    {
        public WebException(int statusCode, string errorString)
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
        public static void ErrorProcess(Exception ex, HttpListenerResponse response)
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
            string responseString = "请求失败！\n" + errorString;
            response.StatusCode = statusCode;
            response.ContentType = "text/plain";
            response.ContentEncoding = Encoding.UTF8;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
