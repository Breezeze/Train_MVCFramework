using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal class FileResponse : IManageResponseMessage
    {
        private string _filePath;

        internal FileResponse(string filePath)
        {
            _filePath = filePath;
        }

        public void ManageResponse(HttpListenerResponse response)
        {
            string filepath = @"..\..\..\Web" + _filePath.Replace('/', '\\');
            if (File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
                string ResponseString = sr.ReadToEnd();
                sr.Dispose();
                response.StatusCode = 200;
                response.ContentType = "text/plan";
                response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = Encoding.UTF8.GetBytes(ResponseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            else
            {
                throw new WebException(404, "未找到该文件！");
            }
        }
    }
}
