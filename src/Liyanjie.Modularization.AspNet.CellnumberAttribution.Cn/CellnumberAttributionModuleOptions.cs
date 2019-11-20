using System;
using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class CellnumberAttributionModuleOptions
    {
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;
    }
}
