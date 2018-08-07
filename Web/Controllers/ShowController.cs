using ListenHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Controllers
{
    public class ShowController : Controller
    {
        public ShowController(HttpListenerContext _context, UrlResult ur) : base(_context, ur)
        {
        }
        public ActionResult index()
        {
            string respon = "";
            if (requestForm.Count != 0)
                respon += context.Request.HttpMethod + "请求参数：\n";
            foreach (var item in requestForm.Keys)
            {
                respon += item + "：" + requestForm[item] + "\n";
            }
            ViewData["感叹号"] = "!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            ViewData.Add("wef_123", ";ljwlefjlkwejflksdafl");
            return View();
        }

        public ActionResult IsGetOrPost()
        {
            string respon = "";
            if (requestForm.Count != 0)
                respon += context.Request.HttpMethod + "请求参数：\n";
            foreach (var item in requestForm.Keys)
            {
                respon += item + "：" + requestForm[item] + "\n";
            }
            respon += "This is a " + context.Request.HttpMethod + " Request.\n";
            return View(respon, true);
        }

    }
}
