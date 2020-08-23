using System;
using System.Threading.Tasks;

using Liyanjie.DataServices;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionGetMiddleware : IMiddleware
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Query.TryGetValue("level", out var _level)
                && Enum.TryParse<AdministrativeDivisionCnLevel>(_level, out var level))
            {
                var data = service.Get(level);

                await options.SerializeToResponseAsync(context.Response, data);
            }
        }
    }
}
