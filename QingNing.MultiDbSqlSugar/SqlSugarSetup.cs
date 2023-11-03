using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QingNing.MultiDbSqlSugar.Extensions;
using SqlSugar;
using System.Collections;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
namespace QingNing.MultiDbSqlSugar;

public static class SqlSugarSetup
{

    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugar(this IServiceCollection services, string dbConnectionOptions = "DbConnectionOptions")
    {
        IConfiguration configuration = App.Configuration;
        var dbOptions = configuration.GetSection(dbConnectionOptions).Get<DbConnectionOptions>();
        dbOptions?.ConnectionConfigs.ForEach(SetDbConfig);

        SqlSugarScope sqlSugar = new(dbOptions?.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
        {
            dbOptions?.ConnectionConfigs.ForEach(config =>
            {
                var dbProvider = db.GetConnectionScope(config.ConfigId);
                SetDbAop(dbProvider);
                SetDbDiffLog(dbProvider, config);
            });
        });

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
        //services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册  

        // 初始化数据库表结构及种子数据
        dbOptions?.ConnectionConfigs.ForEach(config =>
        {
            InitDatabase(sqlSugar, config);
        });

        //services.AddControllers(options =>
        //{
        //    options.Filters.Add<UnitOfWorkAttribute>();
        //});
    }

    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    private static void SetDbConfig(DbConnectionConfig config)
    {
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => // 处理表
            {
                if (config.EnableUnderLine && !entity.DbTableName.Contains('_'))
                    entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            },
            EntityService = (type, column) => // 处理列
            {
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                    column.IsNullable = true;
                if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                    column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                if (config.DbType == SqlSugar.DbType.Oracle)
                {
                    if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                        column.DataType = "number(18)";
                    if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                        column.DataType = "number(1)";
                }
            },
            //DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { };
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true,
            IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
            IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
    }

    /// <summary>
    /// 配置Aop
    /// </summary>
    /// <param name="db"></param>
    public static void SetDbAop(SqlSugarScopeProvider db)
    {
        var config = db.CurrentConnectionConfig;

        // 设置超时时间
        db.Ado.CommandTimeOut = 30;

        // 打印SQL语句
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            var originColor = Console.ForegroundColor;
            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Green;
            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, sql, pars) + "\r\n");
            Console.ForegroundColor = originColor;
        };


        db.Aop.OnError = ex =>
        {
            if (ex.Parametres == null) return;
            var originColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
            Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
            Console.ForegroundColor = originColor;
        };

        // 数据审计
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {
        };
    }

    /// <summary>
    /// 开启库表差异化日志
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config)
    {
        if (!config.EnableDiffLog) return;

        db.Aop.OnDiffLogEvent = async u =>
        {

        };
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
    {
        if (!config.EnableInitDb) return;

        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);

        // 创建数据库
        if (config.DbType != SqlSugar.DbType.Oracle)
            dbProvider.DbMaintenance.CreateDatabase();

        // 获取所有实体表-初始化表结构
        var entityTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(c => c.IsDefined(typeof(SugarTable)))).ToList();

        if (!entityTypes.Any()) return;
        foreach (var entityType in entityTypes)
        {
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;


            var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
            if (splitTable == null)
                dbProvider.CodeFirst.InitTables(entityType);
            else
                dbProvider.CodeFirst.SplitTables().InitTables(entityType);
        }

        if (!config.EnableInitSeed) return;

        // 获取所有种子配置-初始化数据
        var seedDataTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes().Where(c => c.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))))).ToList();

        if (!seedDataTypes.Any()) return;
        foreach (var seedType in seedDataTypes)
        {
            var instance = Activator.CreateInstance(seedType);

            var hasDataMethod = seedType.GetMethod("HasData");
            var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
            if (seedData == null) continue;

            var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
            var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
            if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;


            var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
            if (entityInfo.Columns.Any(u => u.IsPrimarykey))
            {
                // 按主键进行批量增加和更新
                var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                storage.AsInsertable.ExecuteCommand();
                //var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreUpdateAttribute>();
                //if (ignoreUpdate == null) storage.AsUpdateable.ExecuteCommand();
            }
            else
            {
                // 无主键则只进行插入
                if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                    dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
            }
        }
    }

    /// <summary>
    /// 初始化租户业务数据库
    /// </summary>
    /// <param name="iTenant"></param>
    /// <param name="config"></param>
    public static void InitTenantDatabase(ITenant iTenant, DbConnectionConfig config)
    {
        SetDbConfig(config);

        iTenant.AddConnection(config);
        var db = iTenant.GetConnectionScope(config.ConfigId);
        db.DbMaintenance.CreateDatabase();

        //// 获取所有实体表-初始化租户业务表
        //var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
        //    && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(SystemTableAttribute), false)).ToList();
        //if (!entityTypes.Any()) return;
        //foreach (var entityType in entityTypes)
        //{
        //    var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
        //    if (splitTable == null)
        //        db.CodeFirst.InitTables(entityType);
        //    else
        //        db.CodeFirst.SplitTables().InitTables(entityType);
        //}
    }



}