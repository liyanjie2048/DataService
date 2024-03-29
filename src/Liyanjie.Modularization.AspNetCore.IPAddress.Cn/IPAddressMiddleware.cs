﻿using System;
using System.Threading.Tasks;

using Liyanjie.DataService;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class IPAddressMiddleware : IMiddleware
    {
        readonly IPAddressCnService service;
        readonly IPAddressModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public IPAddressMiddleware(
            IPAddressCnService service,
            IOptions<IPAddressModuleOptions> options)
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
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            if (context.Request.Query.TryGetValue("ip", out var ip)&&!string.IsNullOrEmpty(ip))
            {
                var data = service.Find(ip);

                await options.SerializeToResponseAsync(context.Response, data);
            }
        }
    }
}
