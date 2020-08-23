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
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                response.Clear();
                response.ContentType = "application/json";
                await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(obj));
#if NETCOREAPP3_0
                await response.CompleteAsync();
#endif
            };
    }
}
