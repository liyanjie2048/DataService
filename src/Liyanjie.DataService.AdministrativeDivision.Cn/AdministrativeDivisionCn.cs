using System.Collections.Generic;

namespace Liyanjie.DataService
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionCn
    {
        /// <summary>
        /// 
        /// </summary>
        public long Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AdministrativeDivisionCn> Children { get; set; }
    }
}
