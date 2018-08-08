using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenHttp
{
    /// <summary>
    /// 文件请求
    /// </summary>
    internal class FileRequest : IManageRequest
    {
        internal FileRequest() { }
        public IManageResponse ManageRequest(ListenHttpRequest request)
        {
            return new FileResponse(request.UrlResult["filepath"]);
        }
    }
}
