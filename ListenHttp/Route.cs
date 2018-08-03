using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ListenHttp
{
    public class Route
    {
        /// <summary>
        /// 构建路由
        /// </summary>
        private Route(string name, string analysisRule, string defaultUrl)
        {
            _routeName = name;
            //检测路由规则是否符合规范
            //正则表达式：   单个\{[a-z]+\}   整体(\/\{[a-z]+\})+
            Match CorrectRoute = Regex.Match("/" + analysisRule, @"^(\/\{[a-z]+\})+$", RegexOptions.ECMAScript);
            int indexOfController = analysisRule.IndexOf("{controller}");
            int indexOfAction = analysisRule.IndexOf("{action}");
            if (!CorrectRoute.Success || indexOfAction == -1 || indexOfController == -1)
            {
                throw new Exception("路由规则不符合规范 ！");
            }
            AnalysisRule = analysisRule.Split(new char[] { '{', '}', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (name == "Dafualt")
            {
                list.Insert(0, this);
            }
            else
            {
                list.Add(this);
            }
            DefaultUrl = AnalysisUrl(defaultUrl);
        }

        //路由表
        private static List<Route> list = new List<Route>();
        //解析规则(关键词依次排列)
        public string[] AnalysisRule;
        //默认路径
        private UrlResult DefaultUrl;
        //路由名
        private string _routeName;

        /// <summary>
        /// 路由名
        /// </summary>
        public string RouteName { get { return _routeName; } }

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="name"></param>
        /// <param name="analysisRule"></param>
        /// <param name="defaultUrl"></param>
        public static void RegisterRoute(string name, string analysisRule, string defaultUrl)
        {
            new Route(name, analysisRule, defaultUrl);
        }

        /// <summary>
        ///（遍历路由表）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static UrlResult AnalysisUrl(string url)
        {
            foreach (Route route in list)
            {
                UrlResult ur = route.AnalysisUrlOnThis(url);
                if (ur != null)
                {
                    return ur;
                }
            }
            throw new Exception("请检测路径是否正确！", new WebException(400));
        }

        /// <summary>
        /// 解析Url（仅调用本身）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private UrlResult AnalysisUrlOnThis(string url)
        {
            if (url == "/")
            {
                return DefaultUrl;
            }
            else if (System.IO.Path.HasExtension(url.Substring(url.LastIndexOf('/'))))
            {
                return new UrlResult(url);
            }
            else
            {
                //检测路径是否匹配该路由
                string[] parameters = url.Split(new char[] { '{', '}', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (parameters.Length != AnalysisRule.Length)
                {
                    return null;
                }
                //构建分析结果，并返回
                return new ListenHttp.UrlResult(this, AnalysisRule, parameters);
            }
        }


    }
}
