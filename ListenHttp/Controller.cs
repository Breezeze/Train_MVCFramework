using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public static class Controller
    {

        public static Dictionary<string, Type> _subClassList = new Dictionary<string, Type>();
        static Controller()
        {
            if (System.IO.File.Exists("Controllers.dll"))
            {
                List<Type> ctrlTypeList = Assembly.LoadFrom("Controllers.dll").GetTypes().Where(t => t.BaseType == typeof(ControllerBase)).ToList();
                for (int i = 0; i < ctrlTypeList.Count; i++)//将集合中的每项元素存入_subClassList
                {
                    //ControllerBase ctrlClass = (ControllerBase)Activator.CreateInstance(ctrlTypeList[i]);
                    string name = ctrlTypeList[i].Name.Substring(0, ctrlTypeList[i].Name.IndexOf("Controller")).ToLower();
                    _subClassList.Add(name, ctrlTypeList[i]);
                }
            }
            else
            {
                Console.WriteLine("初始化失败！\n未找到程序文件！");
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 根据解析后的url调用Controller的Action
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ur"></param>
        /// <returns></returns>
        public static string InvokingAction(System.Net.HttpListenerContext context, UrlResult ur)
        {
            if (ur.Controller != null)
            {
                if (_subClassList.ContainsKey(ur.Controller))
                {
                    List<MethodInfo> actions = _subClassList[ur.Controller].GetMethods().ToList();
                    object obj = Activator.CreateInstance(_subClassList[ur.Controller]);
                    foreach (MethodInfo item in actions)
                    {
                        if (item.Name.Trim().ToLower().Equals(ur.Action.Trim()))
                        {
                            return (string)item.Invoke(obj, new object[] { context });
                        }
                    }
                }
            }
            return "<html><head><meta charset='utf-8'></head><body>失败！</body></html>";
        }

    }
}
