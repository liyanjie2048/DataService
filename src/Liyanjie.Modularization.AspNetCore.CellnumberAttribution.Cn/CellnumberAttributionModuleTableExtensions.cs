using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class CellnumberAttributionModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="routeTemplate"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddCellnumberAttributionCn(this ModularizationModuleTable moduleTable,
            string routeTemplate = "CA/Find",
            Action<CellnumberAttributionModuleOptions> configureOptions = null)
        {
            moduleTable.Services.AddSingleton<CellnumberAttributionMiddleware>();

            moduleTable.AddModule("CellnumberAttributionModule", new[]
            {
                new ModularizationModuleMiddleware{
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = routeTemplate,
                    HandlerType = typeof(CellnumberAttributionMiddleware),
                },
            }, configureOptions);

            return moduleTable;
        }
    }
}
