using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// C-V传值----ViewData
    /// </summary>
    public class ViewDataBase
    {
        private Dictionary<string, string> ViewData = new Dictionary<string, string>();
        public string this[string name]
        {
            get
            {
                return ViewData.ContainsKey(name) ? ViewData[name] : null;
            }
            set
            {
                ViewData.Add(name, value);
            }
        }
        public void Add(string key, string value)
        {
            ViewData.Add(key, value);
        }
    }
}
