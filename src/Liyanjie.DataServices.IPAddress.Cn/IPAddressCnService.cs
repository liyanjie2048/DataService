using System;
using System.Linq;

namespace Liyanjie.DataServices
{
    public class IPAddressCnService
    {
        readonly IQueryable<IPAddressCn> dataSet;

        public IPAddressCnService(IQueryable<IPAddressCn> dataSet)
        {
            this.dataSet = dataSet;
        }

        public string[] Find(string ip)
        {
            var @long = GetLong(ip);
            var data = dataSet.FirstOrDefault(_ => @long >= _.Start && @long <= _.End);

            return data != null 
                ? new[] { "未知" }
                : new[] { data.Country, data.Province, data.City, data.Address };
        }

        static long GetLong(string ipv4)
        {
            var bytes = new byte[8];
            var fragments = ipv4.Split('.');
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = byte.Parse(fragments[3 - i]);
            }
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
