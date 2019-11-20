using System;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class AdministrativeDivisionModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddAdministrativeDivisionCn(this ModularizationModuleTable moduleTable,
           string getRouteTemplate = "AD/Get",
            string getChildrenRouteTemplate = "AD/GetChildren",
            string findRouteTemplate = "AD/Find",
            Action<AdministrativeDivisionModuleOptions> configureOptions = null)
        {
            moduleTable.AddModule("AdministrativeDivisionModule", new ModularizationModuleMiddleware[]
            {
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = getRouteTemplate,
                    HandlerType = typeof(AdministrativeDivisionGetMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = getChildrenRouteTemplate,
                    HandlerType = typeof(AdministrativeDivisionGetMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = findRouteTemplate,
                    HandlerType = typeof(AdministrativeDivisionGetMiddleware),
                },
            }, configureOptions);

            return moduleTable;
        }
    }
}
