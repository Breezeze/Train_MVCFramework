using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// 处理请求
    /// </summary>
    interface IManageRequest
    {
        IManageResponse ManageRequest(ListenHttpRequest request);
    }
}
