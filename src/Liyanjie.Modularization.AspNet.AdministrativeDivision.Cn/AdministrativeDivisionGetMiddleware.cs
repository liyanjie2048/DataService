using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.DataServices;

using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionGetMiddleware
    {
        readonly AdministrativeDivisionCnService service;
        readonly AdministrativeDivisionModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public AdministrativeDivisionGetMiddleware(
            AdministrativeDivisionCnService service,
            IOptions<AdministrativeDivisionModuleOptions> options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Enum.TryParse<AdministrativeDivisionCnLevel>(context.Request.QueryString["level"], out var level))
            {
                var data = service.Get(level);

                await options.SerializeToResponseAsync(context.Response, data);
            }
        }
    }
}
