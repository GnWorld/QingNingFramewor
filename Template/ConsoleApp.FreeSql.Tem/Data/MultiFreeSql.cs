using ConsoleApp.FreeSqlTemplate.Data.Models;
using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Npgsql;
using QingNing.MultiFreeSql;

namespace ConsoleApp.FreeSqlTemplate.Data;

public enum DbEnum { db1, db2 }
public class MultiFreeSql : FreeSqlCloud<DbEnum>
{
    public MultiFreeSql() : base(null) { }
    public MultiFreeSql(string distributeKey) : base(distributeKey) { }
}

public static class FreeSqlRegister
{
    public static void RegisterFreeSql(this IServiceCollection services, IConfiguration configuration)
    {
        Dictionary<DbEnum, DbConnectionOptionsConfig> dicDbConnections = new()
        {
                { DbEnum.db1, configuration.GetSection("DbConnectionStrings:Db1ConnectionString").Get<DbConnectionOptionsConfig>() },
                { DbEnum.db2, configuration.GetSection("DbConnectionStrings:Db2ConnectionString").Get<DbConnectionOptionsConfig>() },
            };
        var fsql = new MultiFreeSql();
        fsql.DistributeTrace = log => Console.WriteLine(log.Split('\n')[0].Trim());

        fsql.Register(DbEnum.db1, () => new Lazy<IFreeSql>(() =>
        {
            DbConnectionOptionsConfig oFreeSqlDbConnectionItemConfig = dicDbConnections[DbEnum.db1];

            if (oFreeSqlDbConnectionItemConfig?.MasterConnetion != null)
            {
                //NpgsqlConnection.GlobalTypeMapper.UseLegacyPostgis();
                NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();
                var freeBuilder = new FreeSqlBuilder()
                .UseConnectionString(oFreeSqlDbConnectionItemConfig.DataType, oFreeSqlDbConnectionItemConfig.MasterConnetion)
                .UseAutoSyncStructure(false)
                .UseGenerateCommandParameterWithLambda(true)
                .UseLazyLoading(true)
                .UseMonitorCommand(
                    cmd =>
                    {
                        LogHelper.LogInformation(cmd.CommandText);
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
            }
            else
            {
                return null;
            }

        }, true)?.Value);
        fsql.Register(DbEnum.db2, () =>
        {
            DbConnectionOptionsConfig oFreeSqlDbConnectionItemConfig = dicDbConnections[DbEnum.db2];
            var freeSql = new FreeSqlBuilder().UseConnectionString(oFreeSqlDbConnectionItemConfig.DataType, oFreeSqlDbConnectionItemConfig.MasterConnetion).Build();
            return freeSql;
        });
        services.AddSingleton<IFreeSql>(fsql);
        services.AddSingleton(fsql);

    }


}
