using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class UrlResult
    {
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private Route _route;

        /// <summary>
        /// url控制器值
        /// </summary>
        public string Controller { get { return this["controller"]; } }
        /// <summary>
        /// url方法值
        /// </summary>
        public string Action { get { return this["action"]; } }
        /// <summary>
        /// url默认参数id值
        /// </summary>
        public string Id { get { return this["id"]; } }
        /// <summary>
        /// 返回使用的路由
        /// </summary>
        public Route BaseRoute { get { return _route; } }

        internal UrlResult(string filePath)
        {
            _parameters.Add("FilePath".ToLower(), filePath);
        }
        internal UrlResult(Route route, string controller, string action, string id)
        {
            _route = route;
            _parameters.Add("Controller", controller);
            _parameters.Add("Action", action);
            _parameters.Add("id", id.Split('?')[0]);
        }
        internal UrlResult(Route route, string[] parametersName, string[] parameters)
        {
            _route = route;
            for (int i = 0; i < parametersName.Length; i++)
            {
                _parameters.Add(parametersName[i], parameters[i].Split('?')[0]);
            }
        }

        /// <summary>
        /// 检索器，检索参数
        /// </summary>
        public string this[string parameterName]
        {
            get
            {
                return _parameters.ContainsKey(parameterName) ? _parameters[parameterName].ToLower() : null;
            }
        }
    }
}
