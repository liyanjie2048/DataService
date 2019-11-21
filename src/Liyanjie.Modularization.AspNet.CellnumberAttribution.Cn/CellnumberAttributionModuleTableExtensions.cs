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
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddCellnumberAttributionCn(this ModularizationModuleTable moduleTable,
            Action<CellnumberAttributionModuleOptions> configureOptions,
            string routeTemplate = "CA/Find")
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(CellnumberAttributionMiddleware), "Singleton");

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
