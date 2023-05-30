using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using QingNing.MultiFreeSql.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.MultiFreeSql
{
    public class MultiFreeSql : FreeSqlCloud<string>
    {
        public MultiFreeSql() : base(null) { }
        public MultiFreeSql(string distributeKey) : base(distributeKey) { }
    }

    public static class MultiFreeSqlExtenions
    {
        public static void Register(this ServiceCollection services, IConfiguration configuration)
        {
            var connectOptions = configuration.GetSection("DbConnectionStrings").Get<List<ConnectOptions>>();


            var multiFreeSql = new MultiFreeSql();


            foreach (var option in connectOptions)
            {
                multiFreeSql.Register("", () =>
                {
                    var dbTyp = (FreeSql.DataType)Enum.Parse(typeof(FreeSql.DataType), option.DbType);
                    var freeSqlBuilder = new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.PostgreSQL, option.MasterConnection)
                            .UseAutoSyncStructure(false)
                            .UseGenerateCommandParameterWithLambda(true)
                            .UseLazyLoading(true)
                            .UseMonitorCommand(
                                cmd =>
                                {
                                    //LogHelper.Info(cmd.CommandText);
                                }); ;
                    if (option.SlaveConnections?.Count > 0)
                    {
                        freeSqlBuilder.UseSlave(option.SlaveConnections.ToArray());
                    }
                    IFreeSql Fsql = freeSqlBuilder.Build();
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

                });
            }
            services.AddSingleton<IFreeSql>(multiFreeSql);
            services.AddSingleton(multiFreeSql);
        }
    }

}



