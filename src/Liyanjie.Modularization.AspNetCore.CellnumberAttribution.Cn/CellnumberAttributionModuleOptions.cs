using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class CellnumberAttributionModuleOptions
    {
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;
    }
}
