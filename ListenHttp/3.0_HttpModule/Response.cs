using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class ListenHttpResponse
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
        public Encoding ContentEncoding { get { return Encoding.UTF8; } }
        public string ResponseString { get; set; }
        public Dictionary<string, string> Header { get; set; }

        public ListenHttpResponse()
        {
            Header = new Dictionary<string, string>();
        }
        public ListenHttpResponse(int statusCode, string contentType, string responseString)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            ResponseString = responseString;
            Header = new Dictionary<string, string>();
        }
    }
}
