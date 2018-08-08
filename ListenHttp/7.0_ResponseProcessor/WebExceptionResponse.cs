using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal class WebExceptionResponse : IManageResponse
    {
        private ListenHttpResponse _response = new ListenHttpResponse();

        internal WebExceptionResponse(int statusCode, string errorString)
        {
            _response.StatusCode = statusCode;
            _response.ResponseString = errorString;
        }

        public void ManageResponse(HttpListenerResponse response)
        {
            string html = "<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\"></head><body>" + "请求失败！\n" + _response.ResponseString + "</body></html>";
            response.StatusCode = _response.StatusCode;
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
