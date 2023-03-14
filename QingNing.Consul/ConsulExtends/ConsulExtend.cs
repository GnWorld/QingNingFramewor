using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace QingNing.Consul.ConsulExtends
{
    public static class ConsulExtend
    {
        public static void AddConsulRegister(this IServiceCollection services)
        {
            services.ConfigureOptions<ConsulOptions>();

            services.AddTransient<IConsulRegister, ConsulRegister>();

        }

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
    }
}
