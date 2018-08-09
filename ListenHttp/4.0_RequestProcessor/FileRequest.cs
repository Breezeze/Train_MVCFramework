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
    internal class FileRequest : IManageMessageRequest
    {
        internal FileRequest() { }
        public IManageResponseMessage ManageRequest(ListenHttpRequest request)
        {
            return new FileResponse(request.UrlResult["filepath"]);
        }
    }
}
