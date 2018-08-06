using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal class FileResult : ISendResponse
    {
        internal FileResult(string filePath)
        {
            this.filePath = filePath;
        }

        private string filePath;

        public void SendResponse(HttpListenerResponse response)
        {
            string filepath = Listener.WebRootDirectory + filePath.Replace('/', '\\');
            if (File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
                string ResponseString = sr.ReadToEnd();
                sr.Dispose();
                response.StatusCode = 200;
                response.ContentType = "text/plan";
                response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Dispose();
            }
            else
            {
                throw new WebException(404, "未找到该文件！");
            }
        }
    }
}
