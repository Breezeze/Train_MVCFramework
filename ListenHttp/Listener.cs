using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// 封装HttpListener使用
    /// </summary>
    public class Listener
    {
        public Listener()
        {
            sSocket = new HttpListener();
        }

        private HttpListener sSocket;

        public void StartListen(params string[] urls)
        {
            try
            {
                if (urls == null || urls.Length == 0)
                {
                    Console.WriteLine("未设置监视的url！");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                for (int i = 0; i < urls.Length; i++)
                {
                    sSocket.Prefixes.Add(urls[i]);
                }
                sSocket.Start();
                sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("初始化失败！\n" + ex.Message);
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 异步函数
        /// </summary>
        /// <param name="ar"></param>
        private void GetContextCallBack(IAsyncResult ar)
        {
            try
            {
                sSocket = ar.AsyncState as HttpListener;
                HttpListenerContext context = sSocket.EndGetContext(ar);
                sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
                ProcessingRequest_And_RedactResponse(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 结束监听
        /// </summary>
        public void EndListen()
        {
            sSocket.Stop();
            Console.WriteLine("EndListen");
            System.Threading.Thread.Sleep(2000);
        }

        /// <summary>
        /// 处理请求和编写响应
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void ProcessingRequest_And_RedactResponse(HttpListenerContext context)
        {
            //通过路由解析url
            UrlResult ur = Route.AnalysisUrl(context.Request.Url.PathAndQuery);

            //通过url交给对应Controller进行处理，返回响应字符串
            string responseString = Controller.InvokingAction(context, ur);

            //发送响应报文
            SendResponse(context, responseString);
        }

        /// <summary>
        /// 发送响应字符串
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseString"></param>
        private void SendResponse(HttpListenerContext context, string responseString)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }



    }
}
