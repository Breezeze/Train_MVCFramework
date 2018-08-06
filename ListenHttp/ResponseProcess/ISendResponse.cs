using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    public interface ISendResponse
    {
        void SendResponse(System.Net.HttpListenerResponse response);
    }
}
