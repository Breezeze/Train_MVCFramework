using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class ResultAction
    {
        public ResultAction() { }
        public ResultAction(HttpListenerResponse response, string responseString, int statusCode)
        {
            Response = response;
            ResponseString = responseString;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public Encoding ContentEncoding { get { return Encoding.UTF8; } }
        public string ResponseString { get; set; }
        public HttpListenerResponse Response { get; }

        /// <summary>
        /// 发送响应，收尾
        /// </summary>
        /// <param name="context"></param>
        public void SendResponse()
        {
            Response.StatusCode = StatusCode;
            Response.ContentEncoding = ContentEncoding;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
            Response.ContentLength64 = buffer.Length;
            System.IO.Stream output = Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

    }
}
