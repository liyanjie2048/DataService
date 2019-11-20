using System;

namespace Liyanjie.Modularization.AspNet
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
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddCellnumberAttributionCn(this ModularizationModuleTable moduleTable,
            string routeTemplate = "CA/Find",
            Action<CellnumberAttributionModuleOptions> configureOptions = null)
        {
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
