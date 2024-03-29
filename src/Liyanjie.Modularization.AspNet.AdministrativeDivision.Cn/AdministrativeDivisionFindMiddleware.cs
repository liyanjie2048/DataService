﻿using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.DataService;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionFindMiddleware
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
            AdministrativeDivisionModuleOptions options)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var query = context.Request.QueryString;
            if (long.TryParse(query["code"], out var code))
            {
                var ignoreNoName = bool.TryParse(query["ignoreNoName"], out var _absolutely) ? _absolutely : true;
                var trimSuffix = bool.TryParse(query["trimSuffix"], out var _trimSuffix) ? _trimSuffix : true;
                var display = service.Display(code, ignoreNoName, trimSuffix);

                await options.SerializeToResponseAsync(context.Response, display);
            }
        }
    }
}
