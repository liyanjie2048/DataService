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
    public class AdministrativeDivisionGetChildrenMiddleware : IMiddleware
    {
        readonly AdministrativeDivisionCnService service;
        readonly AdministrativeDivisionModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public AdministrativeDivisionGetChildrenMiddleware(
            AdministrativeDivisionCnService service,
            IOptions<AdministrativeDivisionModuleOptions> options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var query = context.Request.Query;
            var code = query.TryGetValue("code", out var _code) && long.TryParse(_code, out var __code)
                ? __code : 0;
            var level = query.TryGetValue("level", out var _level) && Enum.TryParse<AdministrativeDivisionCnLevel>(_level, out var __level)
                ? __level : (AdministrativeDivisionCnLevel)0;
            var data = service.GetChildren(code, level);

            await options.SerializeToResponseAsync(context.Response, data);
        }
    }
}
