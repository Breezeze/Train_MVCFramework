using System.Net;

namespace ListenHttp
{
    internal interface IExecuteResponse
    {
        void ExecuteResponse(HttpListenerResponse response);
    }
}
