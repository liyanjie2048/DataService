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
    public class CellnumberAttributionMiddleware : IMiddleware
    {
        readonly CellnumberAttributionCnService service;
        readonly CellnumberAttributionModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public CellnumberAttributionMiddleware(
            CellnumberAttributionCnService service,
            IOptions<CellnumberAttributionModuleOptions> options)
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
            if (context.Request.Query.TryGetValue("cellnumber", out var cellnumber)
                && !string.IsNullOrEmpty(cellnumber))
            {
                var data = service.Find(cellnumber);

                await options.SerializeToResponseAsync(context.Response, data);
            }
        }
    }
}
