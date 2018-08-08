using ListenHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ListenHttpRequest request) : base(request)
        {

        }

    public ViewResponse index()
        {
            for (int i = 0; i < ctrlAction.Count; i++)
            {
                for (int j = 1; j < ctrlAction[i].Length; j++)
                {
                    ViewData.Add("ControllerAndAction_" + i + "_" + (j - 1), "/" + ctrlAction[i][0] + "/" + ctrlAction[i][j]);
                }
            }
            return View();
        }
        public ViewResponse HelloWorld()
        {
            ViewData.Add("HelloWorld", "HelloWorld!!!");

            return View();
        }
    }
}
