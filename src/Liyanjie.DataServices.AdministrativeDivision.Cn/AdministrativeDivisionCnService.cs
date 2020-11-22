using System;
using System.Collections.Generic;
using System.Linq;

using Level = Liyanjie.DataServices.AdministrativeDivisionCnLevel;

namespace Liyanjie.DataServices
{
    /// <summary>
    /// 
    /// </summary>
    public class AdministrativeDivisionCnService
    {
        const long _province = 1_00_00_000_000L;
        const long _city = 1_00_000_000L;
        const long _county = 1_000_000L;
        const long _town = 1_000L;

        readonly IQueryable<AdministrativeDivisionCn> dataSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        public AdministrativeDivisionCnService(IQueryable<AdministrativeDivisionCn> dataSet)
        {
            this.dataSet = dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="trimSuffix"></param>
        /// <returns></returns>
        public IEnumerable<AdministrativeDivisionCn> Get(Level level, bool trimSuffix = true)
        {
            if (level > Level.Village)
                level = Level.County;

            if (level > 0)
            {
                var data = dataSet.Where(_ => _.Level <= (int)level).ToList();

                return GetChildren(data, 0L, Level.Province).Select(province => new AdministrativeDivisionCn
                {
                    Code = province.Code,
                    Level = province.Level,
                    Display = trimSuffix ? TrimProvinceSuffix(province.Display) : province.Display,
                    Children = level > Level.Province ? GetChildren(data, province.Code, Level.City).Select(city => new AdministrativeDivisionCn
                    {
                        Code = city.Code,
                        Level = city.Level,
                        Display = trimSuffix ? TrimCitySuffix(city.Display) : city.Display,
                        Children = level > Level.City ? GetChildren(data, city.Code, Level.County).Select(county => new AdministrativeDivisionCn
                        {
                            Code = county.Code,
                            Level = county.Level,
                            Display = trimSuffix ? TrimCountySuffix(county.Display) : county.Display,
                            Children = level > Level.County ? GetChildren(data, county.Code, Level.Town).Select(town => new AdministrativeDivisionCn
                            {
                                Code = town.Code,
                                Level = town.Level,
                                Display = trimSuffix ? TrimTownSuffix(town.Display) : town.Display,
                                Children = level > Level.Town ? GetChildren(data, town.Code, Level.Village).Select(village => new AdministrativeDivisionCn
                                {
                                    Code = village.Code,
                                    Level = village.Level,
                                    Display = trimSuffix ? TrimVillageSuffix(village.Display) : village.Display,
                                }) : null,
                            }) : null,
                        }) : null,
                    }) : null,
                });
            }
            else
                return new AdministrativeDivisionCn[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<AdministrativeDivisionCn> GetChildren(long code, Level level)
        {
            return GetChildren(dataSet, code, level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="absolutely"></param>
        /// <returns></returns>
        public bool Validate(long code, bool absolutely = true)
        {
            if (absolutely)
            {
                if (code < 110000000000 || code > 820000000000)
                    return false;
                return dataSet.Any(_ => _.Code == code);
            }

            if (code < 10000000000)
            {
                return long.TryParse(code.ToString().PadRight(12, '0'), out var tmp) && Validate(tmp, true);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ignoreNoName"></param>
        /// <param name="trimSuffix"></param>
        /// <returns></returns>
        public string[] Display(long code, bool ignoreNoName = true, bool trimSuffix = true)
        {
            if (code < 11_00_00_000_000 || code > 82_00_00_000_000)
                throw new ArgumentOutOfRangeException(nameof(code), $"{nameof(code)} 值不在预期范围内。");

            var output = new List<string>();

            var province = dataSet.FirstOrDefault(_ => _.Level == 1 && _.Code == (code / _province * _province))?.Display;
            if (province != null)
            {
                if (trimSuffix)
                    province = TrimProvinceSuffix(province);
                output.Add(province);
            }

            if (code % _province > 0L)
            {
                var city = dataSet.FirstOrDefault(_ => _.Level == 2 && _.Code == (code / _city * _city))?.Display;
                if (city != null)
                    if (false
                        || !ignoreNoName
                        || !("市辖区".Equals(city) || "县".Equals(city) || "省直辖县级行政区划".Equals(city) || "自治区直辖县级行政区划".Equals(city)))
                    {
                        if (trimSuffix)
                            city = TrimCitySuffix(city);
                        output.Add(city);
                    }
            }

            if (code % _city > 0L)
            {
                var county = dataSet.FirstOrDefault(_ => _.Level == 3 && _.Code == (code / _county * _county))?.Display;
                if (county != null)
                    if (false
                        || !ignoreNoName
                        || !"市辖区".Equals(county))
                    {
                        if (trimSuffix)
                            county = TrimCountySuffix(county);
                        output.Add(county);
                    }
            }

            if (code % _county > 0L)
            {
                var town = dataSet.FirstOrDefault(_ => _.Level == 4 && _.Code == (code / _town * _town))?.Display;
                if (town != null)
                {
                    if (trimSuffix)
                        town = TrimTownSuffix(town);
                    output.Add(town);
                }
            }

            if (code % _town > 0L)
            {
                var village = dataSet.FirstOrDefault(_ => _.Level == 5 && _.Code == (code))?.Display;
                if (village != null)
                {
                    if (trimSuffix)
                        village = TrimVillageSuffix(village);
                    output.Add(village);
                }
            }

            return output.ToArray();
        }

        static IEnumerable<AdministrativeDivisionCn> GetChildren(IEnumerable<AdministrativeDivisionCn> data, long code, Level level)
        {
            if (level == Level.Province)
                return data
                    .Where(_ => _.Level == 1)
                    .OrderBy(_ => _.Code)
                    .ToList();

            var _base = level switch
            {
                Level.City => _province,
                Level.County => _city,
                Level.Town => _county,
                Level.Village => _town,
                _ => 1L,
            };

            code = code / _base * _base;
            return data
                .Where(_ => _.Level == (int)level && _.Code > code && _.Code < code + _base)
                .OrderBy(_ => _.Display)
                .ToList();
        }
        static string TrimProvinceSuffix(string province)
        {
            return province
                .Replace("维吾尔自治区", string.Empty)
                .Replace("壮族自治区", string.Empty)
                .Replace("回族自治区", string.Empty)
                .Replace("特别行政区", string.Empty)
                .Replace("自治区", string.Empty)
                .TrimEnd('省', '市')
                ;
        }
        static string TrimCitySuffix(string city)
        {
            return city
                .Replace("布依族苗族自治州", string.Empty)
                .Replace("柯尔克孜自治州", string.Empty)
                .Replace("土家族苗族自治州", string.Empty)
                .Replace("苗族侗族自治州", string.Empty)
                .Replace("哈尼族彝族自治州", string.Empty)
                .Replace("傣族景颇族自治州", string.Empty)
                .Replace("蒙古族藏族自治州", string.Empty)
                .Replace("藏族羌族自治州", string.Empty)
                .Replace("壮族苗族自治州", string.Empty)
                .Replace("傣族自治州", string.Empty)
                .Replace("蒙古自治州", string.Empty)
                .Replace("朝鲜族自治州", string.Empty)
                .Replace("傈僳族自治州", string.Empty)
                .Replace("哈萨克自治州", string.Empty)
                .Replace("藏族自治州", string.Empty)
                .Replace("彝族自治州", string.Empty)
                .Replace("白族自治州", string.Empty)
                .Replace("回族自治州", string.Empty)
                .Replace("自治州", string.Empty)
                .Replace("地区", string.Empty)
                .TrimEnd('盟', '市')
                ;
        }
        static string TrimCountySuffix(string county)
        {
            return county
                .Replace("行政委员会", string.Empty)
                ;
        }
        static string TrimTownSuffix(string town)
        {
            return town
                .Replace("办事处", string.Empty)
                .Replace("建设管理委员会", string.Empty)
                .Replace("管理委员会", string.Empty)
                .Replace("委员会", string.Empty)
                ;
        }
        static string TrimVillageSuffix(string village)
        {
            return village
                .Replace("居民委员会", string.Empty)
                .Replace("居委会", string.Empty)
                .Replace("委会", string.Empty)
                ;
        }
    }
}
