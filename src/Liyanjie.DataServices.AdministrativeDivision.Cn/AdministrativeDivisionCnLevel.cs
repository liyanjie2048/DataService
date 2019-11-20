namespace Liyanjie.DataServices
{
    public enum AdministrativeDivisionCnLevel : uint
    {
        /// <summary>
        /// 省、直辖市、自治区
        /// </summary>
        Province = 1,

        /// <summary>
        /// 市、盟、自治州、地区
        /// </summary>
        City = 2,

        /// <summary>
        /// 区、县
        /// </summary>
        County = 3,

        /// <summary>
        /// 乡、镇、街道办事处
        /// </summary>
        Town = 4,

        /// <summary>
        /// 村
        /// </summary>
        Village = 5,
    }
}
