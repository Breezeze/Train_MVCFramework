using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class ActionResult : ISendResponse
    {
        private ActionResult() { }
        public ActionResult(string responseString, int statusCode, string contextType)
        {
            ResponseString = responseString;
            StatusCode = statusCode;
            ContextType = contextType;
        }

        private int StatusCode { get; }
        private Encoding ContentEncoding { get { return Encoding.UTF8; } }
        private string ResponseString { get; }

        /* ContextType类型：
             text/html ： HTML格式
             text/plain ：纯文本格式      
             text/xml ：  XML格式
             image/gif ：gif图片格式    
             image/jpeg ：jpg图片格式 
             image/png：png图片格式

             以application开头的媒体格式类型：
             application/xhtml+xml ：XHTML格式
             application/xml     ： XML数据格式
             application/atom+xml  ：Atom XML聚合格式    
             application/json    ： JSON数据格式
             application/pdf       ：pdf格式  
             application/msword  ： Word文档格式
             application/octet-stream ： 二进制流数据（如常见的文件下载）  
          */
        private string ContextType;

        /// <summary>
        /// 发送响应，收尾
        /// </summary>
        /// <param name="context"></param>
        public void SendResponse(HttpListenerResponse response)
        {

            response.Headers.Add("Access-Control-Allow-Origin", "*");

            response.StatusCode = StatusCode;
            response.ContentType = ContextType;
            response.ContentEncoding = ContentEncoding;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
