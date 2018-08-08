using System.Net;

namespace ListenHttp
{
    /// <summary>
    /// 处理响应
    /// </summary>
    internal interface IManageResponse
    {
        /// <summary>
        /// 处理响应
        /// </summary>
        void ManageResponse(HttpListenerResponse response);
    }
}
