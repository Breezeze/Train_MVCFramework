using ListenHttp;
using System;

namespace Train_MVCFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 版本1.0

            //HttpListener hl = new HttpListener();
            ////hl.Prefixes.Add("https://www.baidu.com/");
            //hl.Prefixes.Add("http://localhost/");
            //hl.Start();
            //int requestNum = 0;
            //while (true)
            //{
            //    HttpListenerContext hlc = hl.GetContext();
            //    HttpListenerRequest request = hlc.Request;
            //    HttpListenerResponse response = hlc.Response;
            //    //一次请求localhost运行两次，因为浏览器后台请求了一次localhost/favicon.ico（网站头像文件）
            //    requestNum++;
            //    string responseString = string.Format("<html><body>当前时间：{0}<br />请求次数：{1}</body></html>", DateTime.Now, requestNum);
            //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //    response.ContentLength64 = buffer.Length;
            //    System.IO.Stream output = response.OutputStream;
            //    output.Write(buffer, 0, buffer.Length);
            //    output.Close();
            //}
            //hl.Stop(); 

            #endregion


            Listener lis = new Listener();
            lis.StartListen("http://127.0.0.1:8080/", "http://127.0.0.1:8081/");
            Route.RegisterRoute("Default", "{controller}/{action}/{id}", "/home/index/1");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {

            }
            lis.EndListen();
        }
    }
}
