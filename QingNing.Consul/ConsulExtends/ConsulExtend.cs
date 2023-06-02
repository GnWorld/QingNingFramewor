using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QingNing.Consul.ConsulExtends
{
    public static class ConsulExtend
    {

        #region Server 端  
        /// <summary>
        /// 注入Consul配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddConsulRegister(this IServiceCollection services)
        {
            services.AddTransient<IConsulRegister, ConsulRegister>();

        }

        /// <summary>
        /// 使用Consul注册
        /// </summary>
        /// <param name="app"></param>
        public static void UseConsulRegister(this IApplicationBuilder app)
        {
            var consulRegister = app.ApplicationServices.GetRequiredService<IConsulRegister>();
            consulRegister.ConsulRegistAsync().Wait();
        }
        #endregion

        #region Client
        /// <summary>
        /// 注册Consul调度策略
        /// </summary>
        /// <param name="services"></param>
        /// <param name="consulDispatcherType"></param>
        public static void AddConsulDispatcher(this IServiceCollection services, ConsulDispatcherTypeEnum consulDispatcherType)
        {
            switch (consulDispatcherType)
            {
                case ConsulDispatcherTypeEnum.Average:
                    services.AddTransient<AbstractConsulDispatcher, AverageDispatcher>();
                    break;
                case ConsulDispatcherTypeEnum.Polling:
                    services.AddTransient<AbstractConsulDispatcher, PollingDispatcher>();
                    break;
                case ConsulDispatcherTypeEnum.Weight:
                    services.AddTransient<AbstractConsulDispatcher, WeightDispatcher>();
                    break;
                default:
                    break;
            }
        }

        public enum ConsulDispatcherTypeEnum
        {
            Average = 0,
            Polling = 1,
            Weight = 2
        }
        #endregion

    }
}
