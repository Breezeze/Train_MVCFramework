using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class ViewResponse : IManageResponseMessage
    {
        private ListenHttpResponse _response;

        internal ViewResponse(ListenHttpResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// 发送响应，收尾
        /// </summary>
        /// <param name="context"></param>
        public void ManageResponse(HttpListenerResponse response)
        {
            foreach (var item in _response.Header.Keys)
            {
                response.Headers.Add(item, _response.Header[item]);
            }
            if (response.Headers["Access-Control-Allow-Origin"] == null)
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            response.StatusCode = _response.StatusCode;
            response.ContentType = _response.ContentType;
            response.ContentEncoding = _response.ContentEncoding;
            byte[] buffer = Encoding.UTF8.GetBytes(_response.ResponseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
