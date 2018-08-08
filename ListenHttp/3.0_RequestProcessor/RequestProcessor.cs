using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    internal static class RequestProcess
    {

        /// <summary>
        /// 根据解析后的url进行不同的请求处理
        /// </summary>
        internal static IManageRequest ExecuteProcess(UrlResult urlResult)
        {
            if (urlResult["filepath"] != null)
            {
                return new FileRequest();
            }
            else if (urlResult.Controller != null)
            {
                return new ViewRequest();
            }
            else
            {
                throw new WebException(400, "请检测路径的正确性！");
            }
        }




        /// <summary>
        /// 所有控制器的集合
        /// </summary>
        private static Dictionary<string, Type> _subClassList = new Dictionary<string, Type>();

        /// <summary>
        /// 所有控制器名和方法名的集合
        /// string[0]为控制器名，后面的是方法名
        /// </summary>
        internal static List<string[]> ctrlActions = new List<string[]>();

        /// <summary>
        /// 读取dll中Controller
        /// </summary>
        static RequestProcess()
        {
            if (File.Exists("Controllers.dll"))
            {
                List<Type> ctrlTypeList = Assembly.LoadFrom("Controllers.dll").GetTypes().Where(t => t.BaseType == typeof(Controller)).ToList();
                for (int i = 0; i < ctrlTypeList.Count; i++)//将集合中的每项元素存入_subClassList
                {
                    //记录控制器
                    string name = ctrlTypeList[i].Name.Substring(0, ctrlTypeList[i].Name.IndexOf("Controller")).ToLower();
                    _subClassList.Add(name, ctrlTypeList[i]);

                    //记录控制器与控制器中的方法
                    List<MethodInfo> ctrlAction = (from s in ctrlTypeList[i].GetMethods().ToList() where s.ReturnType.Equals(typeof(ViewResponse)) select s).ToList();
                    string[] array = new string[ctrlAction.Count + 1];
                    array[0] = name;
                    for (int j = 1; j < array.Length; j++)
                    {
                        array[j] = ctrlAction[j - 1].Name;
                    }
                    ctrlActions.Add(array);
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
        /// 根据路由找到Controller中对应Action
        /// </summary>
        private static IManageResponse FindAction(ListenHttpRequest request)
        {
            if (_subClassList.ContainsKey(request.UrlResult.Controller))
            {
                List<MethodInfo> actions = _subClassList[request.UrlResult.Controller].GetMethods().ToList();
                object[] construcotParameters = new object[] { request };
                object controllerInstance = Activator.CreateInstance(_subClassList[request.UrlResult.Controller], construcotParameters);
                foreach (MethodInfo item in actions)
                {
                    if (item.Name.ToLower().Equals(request.UrlResult.Action))
                    {
                        object[] actionParameters = new object[] { };
                        return (IManageResponse)item.Invoke(controllerInstance, actionParameters);
                    }
                }
            }
            throw new WebException(404, "未找到该页面！");
        }
    }
}
