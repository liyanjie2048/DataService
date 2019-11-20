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
        /// <param name="options"></param>
        public AdministrativeDivisionCnService(IQueryable<AdministrativeDivisionCn> dataSet)
        {
            this.dataSet = dataSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<AdministrativeDivisionCn> Get(Level level)
        {
            if (level > Level.Village)
                level = Level.County;

            return level > 0 ? GetChildren(0L, Level.Province).Select(province => new AdministrativeDivisionCn
            {
                Code = province.Code,
                Level = province.Level,
                Display = province.Display,
                Children = level > Level.Province ? GetChildren(province.Code, Level.City).Select(city => new AdministrativeDivisionCn
                {
                    Code = city.Code,
                    Level = city.Level,
                    Display = city.Display,
                    Children = level > Level.City ? GetChildren(city.Code, Level.County).Select(county => new AdministrativeDivisionCn
                    {
                        Code = county.Code,
                        Level = county.Level,
                        Display = county.Display,
                        Children = level > Level.County ? GetChildren(county.Code, Level.Town).Select(town => new AdministrativeDivisionCn
                        {
                            Code = town.Code,
                            Level = town.Level,
                            Display = town.Display,
                            Children = level > Level.Town ? GetChildren(town.Code, Level.Village).Select(village => new AdministrativeDivisionCn
                            {
                                Code = village.Code,
                                Level = village.Level,
                                Display = village.Display,
                            }) : null,
                        }) : null,
                    }) : null,
                }) : null,
            }) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public IEnumerable<AdministrativeDivisionCn> GetChildren(long code, Level level)
        {
            if (level == Level.Province)
                return dataSet
                    .Where(_ => _.Level == 1)
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
            return dataSet
                .Where(_ => _.Level == (int)level && _.Code > code && _.Code < code + _base)
                .OrderBy(_ => _.Code)
                .ToList();
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
                output.Add(province);

            if (code % _province > 0L)
            {
                var city = dataSet.FirstOrDefault(_ => _.Level == 2 && _.Code == (code / _city * _city))?.Display;
                if (city != null)
                    if (false
                        || !ignoreNoName
                        || !("市辖区".Equals(city) || "县".Equals(city) || "省直辖县级行政区划".Equals(city) || "自治区直辖县级行政区划".Equals(city)))
                        output.Add(city);
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
                            county = county
                                .Replace("行政委员会", string.Empty)
                                ;
                        output.Add(county);
                    }
            }

            if (code % _county > 0L)
            {
                var town = dataSet.FirstOrDefault(_ => _.Level == 4 && _.Code == (code / _town * _town))?.Display;
                if (town != null)
                {
                    if (trimSuffix)
                        town = town
                            .Replace("办事处", string.Empty)
                            .Replace("建设管理委员会", string.Empty)
                            .Replace("管理委员会", string.Empty)
                            .Replace("委员会", string.Empty)
                            ;
                    output.Add(town);
                }
            }

            if (code % _town > 0L)
            {
                var village = dataSet.FirstOrDefault(_ => _.Level == 5 && _.Code == (code))?.Display;
                if (village != null)
                {
                    if (trimSuffix)
                        village = village
                            .Replace("居民委员会", string.Empty)
                            .Replace("居委会", string.Empty)
                            .Replace("委会", string.Empty);
                    output.Add(village);
                }
            }

            return output.ToArray();
        }
    }
}
