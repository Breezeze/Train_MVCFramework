﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public static class ExecuteController
    {

        public static Dictionary<string, Type> _subClassList = new Dictionary<string, Type>();
        /// <summary>
        /// 读取dll中Controller
        /// </summary>
        static ExecuteController()
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
        public static ActionResult InvokingAction(System.Net.HttpListenerContext context, UrlResult ur)
        {
            if (ur["filepath"] != null)
            {
                ActionResult ar = ProcessFileRequest(ur["filepath"], context.Response);
                if (ar == null)
                {
                    throw new WebException(404, context, "服务器上未找到该文件！");
                }
                else
                {
                    return ar;
                }
            }
            else if (ur.Controller != null)
            {
                if (_subClassList.ContainsKey(ur.Controller))
                {
                    List<MethodInfo> actions = _subClassList[ur.Controller].GetMethods().ToList();
                    object obj = Activator.CreateInstance(_subClassList[ur.Controller], new object[] { context, ur });
                    foreach (MethodInfo item in actions)
                    {
                        if (item.Name.ToLower().Equals(ur.Action))
                        {
                            object[] objParameters = new object[] { };
                            return (ActionResult)item.Invoke(obj, objParameters);
                        }
                    }
                }
                throw new WebException(404, context, "未找到该页面！");
            }
            throw new WebException(400, context, "请检测路径的正确性！");
        }

        public static ActionResult ProcessFileRequest(string _path, HttpListenerResponse response)
        {
            const string rootDirectory = @"..\..\..\..\Web";
            string filepath = rootDirectory + _path.Replace('/', '\\');
            if (File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath, Encoding.UTF8);
                string strhtml = sr.ReadToEnd();
                return new ActionResult(response, strhtml, 200, "octet-stream");
            }
            else
            {
                return null;
            }
        }
    }
}