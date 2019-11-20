using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.DataServices;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class IPAddressMiddleware
    {
        readonly IPAddressCnService service;
        readonly IPAddressModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public IPAddressMiddleware(
            IPAddressCnService service,
            IPAddressModuleOptions options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options ?? new IPAddressModuleOptions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public async Task HandleAsync(HttpContext httpContext)
        {
            var ip = httpContext.Request.QueryString["ip"];
            if (!string.IsNullOrEmpty(ip))
            {
                var data = service.Find(ip);

                await options.SerializeToResponseAsync(httpContext.Response, data);
            }
        }
    }
}
