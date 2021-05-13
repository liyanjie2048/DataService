using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Liyanjie.DataService
{
    /// <summary>
    /// 
    /// </summary>
    public class CellnumberAttributionCnService
    {
        readonly IQueryable<CellnumberAttributionCn> dataSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        public CellnumberAttributionCnService(IQueryable<CellnumberAttributionCn> dataSet)
        {
            this.dataSet = dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellnumber"></param>
        /// <returns></returns>
        public string[] Find(string cellnumber)
        {
            if (string.IsNullOrWhiteSpace(cellnumber))
                throw new ArgumentNullException(nameof(cellnumber));

            if (!Regex.IsMatch(cellnumber, @"^1[3-9]\d{5,9}$"))
                throw new ArgumentException(nameof(cellnumber));

            if (cellnumber.Length > 7)
                cellnumber = cellnumber.Substring(0, 7);

            var data = dataSet.FirstOrDefault(_ => _.Number == cellnumber);

            return data == null
                ? new[] { "未知", "未知", "未知" }
                : new[] { data.Province, data.City, data.Operator };
        }
    }
}
