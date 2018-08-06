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
            if (requestForm.Count != 0)
                Console.WriteLine(context.Request.HttpMethod + "请求参数：");
            foreach (var item in requestForm.Keys)
            {
                Console.WriteLine(item + "：" + requestForm[item]);
            }
            ViewData["感叹号"] = "!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            ViewData.Add("wef_123", ";ljwlefjlkwejflksdafl");
            return View();
        }

        public ActionResult IsGetOrPost()
        {
            if (requestForm.Count != 0)
                Console.WriteLine(context.Request.HttpMethod + "请求参数：");
            foreach (var item in requestForm.Keys)
            {
                Console.WriteLine(item + "：" + requestForm[item]);
            }
            string respon = "This is a " + context.Request.HttpMethod + " Request.\n";
            return View(respon, true);
        }

    }
}
