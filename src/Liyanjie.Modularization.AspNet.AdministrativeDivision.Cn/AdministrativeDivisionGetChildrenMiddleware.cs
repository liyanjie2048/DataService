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
    public class AdministrativeDivisionGetChildrenMiddleware
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.QueryString;
            var code = long.TryParse(query["code"], out var _code) ? _code : 0;
            var level = Enum.TryParse<AdministrativeDivisionCnLevel>(query["level"], out var _level) ? _level : (AdministrativeDivisionCnLevel)0;
            var data = service.GetChildren(code, level);

            await options.SerializeToResponseAsync(context.Response, data);
        }
    }
}
