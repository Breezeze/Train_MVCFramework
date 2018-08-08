using ListenHttp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ListenHttp
{
    public class ListenHttpRequest
    {
        public string HttpMethod { get; }
        public Uri Url { get; }
        public UrlResult UrlResult { get; }
        public Stream InputStream { get; }
        public string ContentType { get; }
        public Encoding ContentEncoding { get; }
        public NameValueCollection Headers { get; }
        public Dictionary<string, string> Form { get; }

        public ListenHttpRequest(UrlResult urlResult, HttpListenerRequest request)
        {
            UrlResult = urlResult;
            HttpMethod = request.HttpMethod;
            Url = request.Url;
            InputStream = request.InputStream;
            ContentType = request.ContentType;
            ContentEncoding = request.ContentEncoding;
            Headers = request.Headers;
            Form = new Dictionary<string, string>();
            ExtractParameters(request.QueryString);
            Console.WriteLine("QueryString.Count=" + request.QueryString.Count);
            Console.WriteLine("Headers.Count=" + Headers.Count);
            Console.WriteLine("Parameters.Count=" + Form.Count);
        }

        /// <summary>
        /// 提取请求报文的参数
        /// </summary>
        private void ExtractParameters(NameValueCollection queryString)
        {
            //区分Get-Post请求
            string httpMethod = HttpMethod.ToUpper();
            if (httpMethod == "POST")
            {
                using (Stream inputStream = InputStream)
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
                            Form.Add(item.Key, item.Value);
                        }
                    }
                    catch (Exception)
                    {
                        string[] parameters = json.Split(new char[] { '&', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parameters.Length % 2 == 0)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                Form.Add(parameters[i], parameters[++i]);
                            }
                        }
                    }
                }
            }
            else if (httpMethod == "GET")
            {
                for (int i = 0; i < queryString.Count; i++)
                {
                    Form.Add(queryString.Keys[i], queryString[i]);
                }
            }
        }
    }
}
