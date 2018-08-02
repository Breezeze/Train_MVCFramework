using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class TryController : ListenHttp.ControllerBase
    {
        public string tryAction(HttpListenerContext context)
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            respon += "参数有：<br />";
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                respon += context.Request.QueryString.Keys[i] + "： ";
                respon += context.Request.QueryString[i] + "<br />";
            }
            respon += "</body></html>";
            return respon;
        }
        public string index(HttpListenerContext context)
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            respon += "参数有：<br />";
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                respon += context.Request.QueryString[i] + "<br />";
            }
            respon += "</body></html>";
            return respon;
        }

        public string home(HttpListenerContext context)
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />成功！";
            respon += "</body></html>";
            return respon;
        }

    }
}
