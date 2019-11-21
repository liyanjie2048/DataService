using System;

namespace Liyanjie.Modularization.AspNet
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
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddCellnumberAttributionCn(this ModularizationModuleTable moduleTable,
            Action<IPAddressModuleOptions> configureOptions,
            string routeTemplate = "IP/Find")
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(IPAddressMiddleware), "Singleton");

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
