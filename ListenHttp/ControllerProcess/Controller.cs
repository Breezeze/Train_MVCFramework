using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ListenHttp
{
    public abstract class Controller : ControllerBase
    {
        public Controller(HttpListenerContext _context, UrlResult ur) : base(_context, ur)
        {
            ExtractParameters();
        }

        /// <summary>
        /// 请求参数列表
        /// </summary>
        protected Dictionary<string, string> requestForm = new Dictionary<string, string>();

        /// <summary>
        /// 请求参数列表
        /// </summary>
        protected string GetrequestForm(string key)
        {
            return requestForm.ContainsKey(key) ? requestForm[key] : null;
        }

        /// <summary>
        /// 提取请求报文的参数
        /// </summary>
        private void ExtractParameters()
        {
            //区分Get-Post请求
            string httpMethod = context.Request.HttpMethod.ToUpper();
            if (httpMethod == "POST")
            {
                using (Stream inputStream = context.Request.InputStream)
                {
                    byte[] buffer = new byte[1024];
                    int jsonLength = inputStream.Read(buffer, 0, buffer.Length);
                    if (jsonLength <= 0)
                    {
                        return;
                    }
                    string json = Encoding.UTF8.GetString(buffer, 0, jsonLength);
                    try
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        dynamic modelDy = js.Deserialize<dynamic>(json); //反序列化
                        foreach (var item in modelDy)
                        {
                            requestForm.Add(item.Key, item.Value);
                        }
                    }
                    catch (Exception)
                    {
                        string[] parameters = json.Split(new char[] { '&', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parameters.Length % 2 == 0)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                requestForm.Add(parameters[i], parameters[++i]);
                            }
                        }
                    }
                }
            }
            else if (httpMethod == "GET")
            {
                for (int i = 0; i < Request.QueryString.Count; i++)
                {
                    requestForm.Add(Request.QueryString.Keys[i], Request.QueryString[i]);
                }
            }
        }

        /// <summary>
        /// 写入响应流数据
        /// </summary>
        protected void ResponseWrite(string responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            Response.ContentLength64 = buffer.Length;
            System.IO.Stream output = Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
        }

    }
}
