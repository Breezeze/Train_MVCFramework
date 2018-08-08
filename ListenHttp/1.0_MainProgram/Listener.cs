using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// 封装HttpListener使用
    /// </summary>
    public class Listener
    {

        private HttpListener sSocket;

        public Listener(params string[] urls)
        {
            sSocket = new HttpListener();
            if (!HttpListener.IsSupported)
            {
                throw new Exception("无法在当前系统上运行服务(Windows XP SP2 or Server 2003)。");
            }
            if (urls == null || urls.Length == 0)
            {
                throw new Exception("未设置监视的url！");
            }
            for (int i = 0; i < urls.Length; i++)
            {
                sSocket.Prefixes.Add(urls[i]);
            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartListen()
        {
            Console.WriteLine("开始监听！");
            sSocket.Start();
            sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
        }

        /// <summary>
        /// 异步函数
        /// </summary>
        private void GetContextCallBack(IAsyncResult ar)
        {
            //System.Diagnostics.Stopwatch executeTime = new System.Diagnostics.Stopwatch();
            //executeTime.Start();
            //executeTime.Stop();
            //Console.WriteLine(executeTime.Elapsed.TotalSeconds * 1000);
            sSocket = ar.AsyncState as HttpListener;
            HttpListenerContext context = sSocket.EndGetContext(ar);
            TreatmentScheme(context);
            sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
        }

        /// <summary>
        /// 处理请求和编写响应
        /// </summary>
        private void TreatmentScheme(HttpListenerContext context)
        {
            try
            {
                //通过url交给对应Controller进行处理，返回发送响应报文类
                IExecuteResponse executeResponse = RequestProcess.ExecuteProcess(context);

                //发送响应报文
                executeResponse.ExecuteResponse(context.Response);
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
