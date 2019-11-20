using System;
using System.Threading.Tasks;
using System.Web;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class IPAddressModuleOptions
    {
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;
    }
}
