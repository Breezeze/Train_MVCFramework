using ListenHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Controllers
{
    public class HomeController : ListenHttp.ControllerBase
    {
        public HomeController(HttpListenerContext _context, UrlResult ur) : base(_context, ur)
        {
        }


        public ActionResult index()
        {
            ViewData["感叹号"] = "!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            ViewData.Add("wef_123", ";ljwlefjlkwejflksdafl");
            return View();
        }
        public ActionResult IsGetOrPost()
        {
            string respon = "经过判别，这是一个" + (isGet ? "Get" : "Post") + "请求。\n";
            if (!isGet)
            {
                respon += "你成功的Post了一个请求！\n";
            }
            return View(respon, true);
        }



    }
}
