using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class IPAddressModuleOptions
    {
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;
    }
}
