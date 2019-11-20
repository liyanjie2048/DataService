﻿using System;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class IPAddressModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddCellnumberAttributionCn(this ModularizationModuleTable moduleTable,
            string routeTemplate = "IP/Find",
            Action<IPAddressModuleOptions> configureOptions = null)
        {
            moduleTable.AddModule("IPAddressModule", new[] 
            {
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = routeTemplate,
                    HandlerType = typeof(IPAddressMiddleware),
                },
            }, configureOptions);

            return moduleTable;
        }
    }
}
