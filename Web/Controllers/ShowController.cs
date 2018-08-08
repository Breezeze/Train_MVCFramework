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
        public ShowController(ListenHttpRequest request) : base(request)
        {
        }

        public ViewResponse index()
        {
            string respon = "";
            if (Request.Form.Count != 0)
                respon += Request.HttpMethod + "请求参数：<br />";
            foreach (var item in Request.Form.Keys)
            {
                respon += Request.Form[item] + "<br />";
            }
            response.ResponseString = respon;
            ViewData["感叹号"] = "!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            ViewData.Add("wef_123", ";ljwlefjlkwejflksdafl");
            return View();
        }

        public ViewResponse IsGetOrPost()
        {
            string respon = "";
            if (Request.Form.Count != 0)
                respon += "QueryString:\n";
            foreach (var item in Request.Form.Keys)
            {
                respon += Request.Form[item] + "\n";
            }
            respon += "This is a " + Request.HttpMethod + " Request.\n";
            return View(respon, true);
        }

    }
}
