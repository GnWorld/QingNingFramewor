using FreeSql;
using FreeSql.DataAnnotations;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using QingNing.MultiFreeSql.Attributes;

namespace FreeSqlTest.Services;
public interface ITestService
{
    Task Test();
}

public class TestService : ITestService
{
    private readonly IBaseRepository<AppRole> _roleRep;
    private readonly IBaseRepository<RoutesOptimized> _routesOptimizedRep;

    public TestService(IBaseRepository<AppRole> roleRep, IBaseRepository<RoutesOptimized> routesOptimizedRep)
    {
        _roleRep = roleRep;
        _routesOptimizedRep = routesOptimizedRep;
    }

    //[UnitOfWork]
    public async Task Test()
    {
        try
        {
            //var role = new AppRole()
            //{

            //    RoleName = "test",
            //    Code = "Test",
            //};

            //await _roleRep.InsertAsync(role);

            //var role2 = new AppRole()
            //{
            //    RoleName = null,
            //    Code = "Test",
            //};

            //await _roleRep.InsertAsync(role2);
            //await _roleRep.Context.Ado.ExecuteCommandAsync("delete from AppRole");

            var routes = await _routesOptimizedRep.Select.Limit(10).ToListAsync();

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.ToString());
            throw;
        }

    }

    [Table(Name = "app_role")]
    public class AppRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty, Column(Name = "role_id", IsPrimary = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty, Column(Name = "role_name", DbType = "varchar(60)")]
        public string RoleName { get; set; } = string.Empty;

        [Column(IsNullable = false)]
        public string Code { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn), Table(Name = "routes_optimized")]
    public partial class RoutesOptimized
    {

        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty, Column(Name = "id", IsPrimary = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 航司三字码
        /// </summary>
        [JsonProperty, Column(Name = "cmy_code", DbType = "varchar(5)")]
        public string CmyCode { get; set; } = string.Empty;

        /// <summary>
        /// 优化航路名称，唯一
        /// </summary>
        [JsonProperty, Column(Name = "name", DbType = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 原始航路走向
        /// </summary>
        [JsonProperty, Column(Name = "route_dir_original", DbType = "text")]
        public string RouteDirOriginal { get; set; } = string.Empty;

        /// <summary>
        /// 原始航路走向点集合的geojson字符串
        /// </summary>
        [JsonProperty, Column(Name = "route_dir_original_geo")]
        public LineString? RouteDirOriginalGeo { get; set; }

        /// <summary>
        /// 原始航路走向距离
        /// </summary>
        [JsonProperty, Column(Name = "distance_original")]
        public double DistanceOriginal { get; set; }

        /// <summary>
        /// 优化后航路走向
        /// </summary>
        [JsonProperty, Column(Name = "route_dir_optimized", DbType = "text")]
        public string RouteDirOptimized { get; set; } = string.Empty;

        /// <summary>
        /// 优化后航路走向点集合的geojson字符串
        /// </summary>
        [JsonProperty, Column(Name = "route_dir_optimized_geo")]
        public LineString? RouteDirOptimizedGeo { get; set; }

        /// <summary>
        /// 优化后航路走向距离
        /// </summary>
        [JsonProperty, Column(Name = "distance_optimized")]
        public double DistanceOptimized { get; set; }

        /// <summary>
        /// 公司涉及城市对
        /// </summary>
        [JsonProperty, Column(Name = "city_pairs_cmy", DbType = "text")]
        public string CityPairsCmy { get; set; } = string.Empty;

        /// <summary>
        /// 行业涉及城市对
        /// </summary>
        [JsonProperty, Column(Name = "city_pairs_industry", DbType = "text")]
        public string CityPairsIndustry { get; set; } = string.Empty;

        /// <summary>
        /// 分析区域点名称集合以空格隔开
        /// </summary>
        [JsonProperty, Column(Name = "points_area_analyse", DbType = "text")]
        public string PointsAreaAnalyse { get; set; } = string.Empty;

        /// <summary>
        /// 分析区域的Polygen
        /// </summary>
        [JsonProperty, Column(Name = "area_analyse_geo")]
        public Polygon? AreaAnalyseGeo { get; set; }

        [JsonProperty, Column(Name = "time_range_end")]
        public DateTime TimeRangeEnd { get; set; }

        [JsonProperty, Column(Name = "time_range_start")]
        public DateTime TimeRangeStart { get; set; }

    }
}