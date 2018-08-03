using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public abstract class ControllerBase
    {
        protected HttpListenerContext context { get; }
        protected HttpListenerRequest request { get; }
        protected HttpListenerResponse response { get; }

        protected ViewDataBase ViewData;


        protected ResultAction View(string viewUrl)
        {
            return null;
        }





        public class ViewDataBase
        {
            private Dictionary<string, object> ViewData = new Dictionary<string, object>();
            public object this[string name]
            {
                get
                {
                    return ViewData[name];
                }
                set
                {
                    ViewData[name] = value;
                }
            }
        }
    }

}
