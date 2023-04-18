using QingNing.FreeSql_Net6;

namespace TestWebApi
{
    public static class FreeSqlStartup
    {
        /// <summary>
        /// FreeSQL注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddFreeSQLConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var multiFreeSQL = new MultiFreeSql();
            multiFreeSQL.RegisterDbSet(configuration);
            services.AddSingleton<IBaseMultiFreeSql<DbEnum>>(multiFreeSQL);
            //services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);      //注入FreeSQL Repository 
        }
    }
}
