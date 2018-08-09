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

        private HttpListener _httpListener;

        public Listener(params string[] urls)
        {
            _httpListener = new HttpListener();
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
                _httpListener.Prefixes.Add(urls[i]);
            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartListen()
        {
            Console.WriteLine("开始监听！");
            _httpListener.Start();
            _httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), _httpListener);
        }

        /// <summary>
        /// 异步函数
        /// </summary>
        private void GetContextCallBack(IAsyncResult ar)
        {
            System.Diagnostics.Stopwatch executeTime = new System.Diagnostics.Stopwatch();
            executeTime.Start();

            _httpListener = ar.AsyncState as HttpListener;
            HttpListenerContext context = _httpListener.EndGetContext(ar);
            TreatmentScheme(context);

            executeTime.Stop();
            Console.WriteLine("该请求处理时间：" + executeTime.Elapsed.TotalSeconds * 1000 + "毫秒。");

            _httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), _httpListener);
        }

        /// <summary>
        /// 处理请求和编写响应
        /// </summary>
        private void TreatmentScheme(HttpListenerContext context)
        {
            try
            {
                //客户端访问记录
                Console.WriteLine("\n客户端发出" + context.Request.HttpMethod + "请求：" + context.Request.Url);

                //封装HttpListenerRequest
                ListenHttpRequest request = new ListenHttpRequest(context.Request);

                //解析URL，获取对应的请求处理类
                IManageMessageRequest manageRequest = RequestProcess.ExecuteProcess(request.UrlResult);

                //根据不同请求处理类，获取不同的响应报文处理类
                IManageResponseMessage manageResponse = manageRequest.ManageRequest(request);

                //处理并发送响应报文
                manageResponse.ManageResponse(context.Response);
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
            _httpListener.Stop();
            Console.WriteLine("EndListen");
            System.Threading.Thread.Sleep(2000);
        }
    }
}
