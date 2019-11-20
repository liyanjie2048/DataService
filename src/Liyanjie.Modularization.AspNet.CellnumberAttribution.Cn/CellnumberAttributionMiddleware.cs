using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.DataServices;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class CellnumberAttributionMiddleware
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
            CellnumberAttributionModuleOptions options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options ?? new CellnumberAttributionModuleOptions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public async Task HandleAsync(HttpContext httpContext)
        {
            var cellnumber = httpContext.Request.QueryString["cellnumber"];
            if (!string.IsNullOrEmpty(httpContext.Request.QueryString["cellnumber"]))
            {
                var data = service.Find(cellnumber);

                await options.SerializeToResponseAsync(httpContext.Response, data);
            }
        }
    }
}
