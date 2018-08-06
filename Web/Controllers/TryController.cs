using ListenHttp;
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
        public TryController(HttpListenerContext _context, UrlResult ur) : base(_context, ur)
        {
        }

        public ActionResult tryAction()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                if (i == 0)
                {
                    respon += "参数有：<br />";
                }
                respon += context.Request.QueryString[i] + "<br />";
            }
            respon += "</body></html>";
            return View(respon, true);
        }
        public ActionResult index()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                if (i == 0)
                {
                    respon += "参数有：<br />";
                }
                respon += context.Request.QueryString.Keys[i] + "： ";
                respon += context.Request.QueryString[i] + "<br />";
            }
            respon += "</body></html>";
            return View(respon, true);
        }

        public ActionResult home()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />成功！";
            respon += "</body></html>";
            return View(respon, true);
        }
        public ActionResult HtmlPage()
        {
            return View();
        }
    }
}
