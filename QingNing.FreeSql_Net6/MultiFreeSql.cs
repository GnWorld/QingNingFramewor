using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using FreeSql;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace QingNing.FreeSql_Net6
{
    //《使用示例》

    ///// <summary>
    ///// FreeSQL注入
    ///// </summary>
    ///// <param name="services"></param>
    ///// <param name="configuration"></param>
    ///// <exception cref="ArgumentNullException"></exception>
    //public static void AddFreeSQLConfiguration(this IServiceCollection services, IConfiguration configuration)
    //{
    //    if (services == null) throw new ArgumentNullException(nameof(services));
    //    var multiFreeSQL = new MultiFreeSql();
    //    multiFreeSQL.RegisterDbSet(configuration);
    //    services.AddSingleton<IBaseMultiFreeSql<DbEnum>>(multiFreeSQL);
    //    //services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);      //注入FreeSQL Repository 
    //}
    
      // Json配置
      //    "DbConnectionStrings": {
      //  "DbAppPostgresqlConnectionString": {
      //    "MasterConnetion": "Host=172.30.7.100;Port=5000;Username=admin;Password=postgres;Database=adf_db_app;Pooling=true;Maximum Pool Size=5",
      //    "SlaveConnections": [
      //      "Host=172.30.7.100;Port=5001;Username=admin;Password=postgres;Database=adf_db_app;Pooling=true;Maximum Pool Size=5",
      //      "Host=172.30.7.100;Port=5001;Username=admin;Password=postgres;Database=adf_db_app;Pooling=true;Maximum Pool Size=5"
      //    ]
      //  }
      //},





    public enum DbEnum
    {
        [Display(Description = "应用程序数据库", Name = "adf_db_app")]
        DbApp,
        [Display(Description = "其他测试数据库", Name = "adf_db_other")]
        DbOther
    }
    public class MultiFreeSql : BaseMultiFreeSql<DbEnum>
    {
        private readonly Logger<MultiFreeSql> logger;

        public MultiFreeSql(Logger<MultiFreeSql> logger)
        {
            this.logger = logger;
        }

        public void RegisterDbSet(IConfiguration configuration)
        {
            Dictionary<DbEnum, DbConnectionOptionsConfig> dicDbConnections = new()
            {
                { DbEnum.DbApp, configuration.GetSection("DbConnectionStrings:DbAppPostgresqlConnectionString").Get<DbConnectionOptionsConfig>() },
                { DbEnum.DbOther, configuration.GetSection("DbConnectionStrings:DbOtherPostgresqlConnectionString").Get<DbConnectionOptionsConfig>() }
            };
            //注册数据库DbApp
            if (dicDbConnections[DbEnum.DbApp] != null)
                Register(DbEnum.DbApp, () => new Lazy<IFreeSql>(() =>
                {
                    DbConnectionOptionsConfig oFreeSqlDbConnectionItemConfig = dicDbConnections[DbEnum.DbApp];

                    //NpgsqlConnection.GlobalTypeMapper.UseLegacyPostgis();
                    //NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();
                    var freeBuilder = new FreeSqlBuilder()
                    .UseConnectionString(FreeSql.DataType.PostgreSQL, oFreeSqlDbConnectionItemConfig.MasterConnetion)
                    .UseAutoSyncStructure(false)
                    .UseGenerateCommandParameterWithLambda(true)
                    .UseLazyLoading(true)
                    .UseMonitorCommand(
                        cmd =>
                        {
                            logger.LogInformation(cmd.CommandText);
                            //LogHelper.Info(cmd.CommandText);
                        });
                    //是否开启读写分离配置
                    if (oFreeSqlDbConnectionItemConfig.SlaveConnections?.Count > 0)//判断是否存在从库
                    {
                        freeBuilder.UseSlave(oFreeSqlDbConnectionItemConfig.SlaveConnections.ToArray());
                    }
                    IFreeSql Fsql = freeBuilder.Build();
                    Fsql.Aop.CurdBefore += (s, e) =>
                    {
                        string strSQL = e.Sql;
                    };
                    Fsql.Aop.CurdAfter += (s, e) =>
                    {
                        if (e.ElapsedMilliseconds > 200)
                        {
                            //记录日志
                            //发送短信给负责人
                        }
                    };
                    //敏感词汇审计
                    Fsql.Aop.AuditValue += (s, e) =>
                    {
                    };
                    ////我这里禁用了导航属性联级插入的功能:因为很难理解，我们可以直接用事务来自己实现，原理是一样的
                    //Fsql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = false);
                    //FreeSql全局过滤
                    return Fsql;


                }, true)?.Value);
            //注册数据库DbOther
            if (dicDbConnections[DbEnum.DbOther] != null)
                Register(DbEnum.DbOther, () => new Lazy<IFreeSql>(() =>
                {
                    DbConnectionOptionsConfig oFreeSqlDbConnectionItemConfig = dicDbConnections[DbEnum.DbOther];

                    //NpgsqlConnection.GlobalTypeMapper.UseLegacyPostgis();
                    var freeBuilder = new FreeSqlBuilder()
                    .UseConnectionString(FreeSql.DataType.PostgreSQL, oFreeSqlDbConnectionItemConfig.MasterConnetion)
                    .UseAutoSyncStructure(false)
                    .UseGenerateCommandParameterWithLambda(true)
                    .UseLazyLoading(true)
                    .UseMonitorCommand(
                        cmd =>
                        {

                        });
                    //是否开启读写分离配置
                    if (oFreeSqlDbConnectionItemConfig.SlaveConnections?.Count > 0)//判断是否存在从库
                    {
                        freeBuilder.UseSlave(oFreeSqlDbConnectionItemConfig.SlaveConnections.ToArray());
                    }
                    IFreeSql Fsql = freeBuilder.Build();
                    Fsql.Aop.CurdBefore += (s, e) =>
                    {
                        string strSQL = e.Sql;
                    };
                    Fsql.Aop.CurdAfter += (s, e) =>
                    {
                        if (e.ElapsedMilliseconds > 200)
                        {
                            //记录日志
                            //发送短信给负责人
                        }
                    };
                    //敏感词汇审计
                    Fsql.Aop.AuditValue += (s, e) =>
                    {
                    };
                    ////我这里禁用了导航属性联级插入的功能:因为很难理解，我们可以直接用事务来自己实现，原理是一样的
                    //Fsql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = false);
                    ///FreeSql全局过滤
                    //Fsql.GlobalFilter.ApplyOnly<OtrItem>("test3", a => a.Name == "11");
                    ///FreeSql多租户
                    //1.租户定义
                    Fsql.GlobalFilter.ApplyIf<ITenant>(
                                            "TenantFilter", // 过滤器名称
                                            () => TenantManager.Current > 0, // 过滤器生效判断
                                            a => a.TenantId == TenantManager.Current // 过滤器条件
                                        );
                    return Fsql;

                }, true)?.Value);
        }
    }
}
