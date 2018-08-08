using ListenHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class TryController : Controller
    {
        public TryController(ListenHttpRequest request) : base(request)
        {
        }

        public ViewResponse tryAction()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            if (Request.Form.Count != 0)
            {
                respon += "参数有：<br />";
            }
            foreach (var item in Request.Form.Keys)
            {
                respon += Request.Form[item] + "<br />";
            }
            respon += "</body></html>";
            return View(respon, true);
        }

        public ViewResponse index()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />";
            if (Request.Form.Count != 0)
            {
                respon += "参数有：<br />";
            }
            foreach (var item in Request.Form.Keys)
            {
                respon += Request.Form[item] + "<br />";
            }
            respon += "</body></html>";
            return View(respon, true);
        }

        public ViewResponse home()
        {
            string respon = "<html><head><meta charset='utf-8'></head><body>当前时间：";
            respon += DateTime.Now + "<br />成功！";
            respon += "</body></html>";
            return View(respon, true);
        }

        public ViewResponse HtmlPage()
        {
            return View();
        }
    }
}
