using System;
using System.Threading.Tasks;

using Liyanjie.DataService;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionFindMiddleware : IMiddleware
    {
        readonly AdministrativeDivisionCnService service;
        readonly AdministrativeDivisionModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public AdministrativeDivisionFindMiddleware(
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
            var query = context.Request.Query;
            if (query.TryGetValue("code", out var _code) && long.TryParse(_code, out var code))
            {
                var ignoreNoName = bool.TryParse(query["ignoreNoName"], out var _absolutely) ? _absolutely : true;
                var trimSuffix = bool.TryParse(query["trimSuffix"], out var _trimSuffix) ? _trimSuffix : true;
                var display = service.Display(code, ignoreNoName, trimSuffix);

                await options.SerializeToResponseAsync(context.Response, display);
            }
        }
    }
}
