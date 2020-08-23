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
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                await Task.FromResult(0);

                response.Clear();
                response.ContentType = "application/json";
                response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                response.End();
            };
    }
}
