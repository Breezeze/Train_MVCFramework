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
        public Listener(string webRootDirectory)
        {
            _wrd = webRootDirectory;
            sSocket = new HttpListener();
        }

        private HttpListener sSocket;
        private static string _wrd;
        public static string WebRootDirectory { get { return _wrd; } }

        /// <summary>
        /// 开始监听
        /// </summary>
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
        private void GetContextCallBack(IAsyncResult ar)
        {
            sSocket = ar.AsyncState as HttpListener;
            HttpListenerContext context = sSocket.EndGetContext(ar);
            sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
            CoreProcess(context);
        }

        /// <summary>
        /// 处理请求和编写响应
        /// </summary>
        private void CoreProcess(HttpListenerContext context)
        {
            try
            {
                //客户端访问记录
                Console.WriteLine("客户端发出请求：" + context.Request.Url);

                //通过url交给对应Controller进行处理，返回发送响应报文类
                ISendResponse sr = RequestProcess.ExecuteProcess(context);

                //发送响应报文
                sr.SendResponse(context.Response);
            }
            catch (Exception ex)
            {
                WebException.ErrorProcess(ex, context.Response);
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





    }
}
