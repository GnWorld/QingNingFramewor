using Consul;
using Microsoft.Extensions.Options;

namespace QingNing.Consul
{
    public class ConsulRegister : IConsulRegister
    {
        private readonly ConsulOptions _consulOptions;

        public ConsulRegister(IOptionsMonitor<ConsulOptions> consulRegisterOptions)
        {
            _consulOptions = consulRegisterOptions.CurrentValue;
        }

        public async Task ConsulRegistAsync()
        {
            var client = new ConsulClient(options =>
            {
                options.Address = new Uri(_consulOptions.Address); // Consul客户端地址
            });

            var registration = new AgentServiceRegistration
            {
                ID = Guid.NewGuid().ToString(), // 唯一Id
                Name = _consulOptions.Name, // 服务名(分组--多个实例组成的集群)
                Address = _consulOptions.Ip, // 服务绑定IP
                Port = Convert.ToInt32(_consulOptions.Port), // 服务绑定端口
                //Tag 标签
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // 服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10), // 健康检查时间间隔
                    HTTP = $"http://{_consulOptions.Ip}:{_consulOptions.Port}{_consulOptions.HealthCheck}", // 健康检查地址
                    Timeout = TimeSpan.FromSeconds(5) // 超时时间
                }
            };

            await client.Agent.ServiceRegister(registration);
        }
    }
}
