using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            var client = new ConsulClient();
            var registration = new AgentServiceRegistration();
            registration.ID = Guid.NewGuid().ToString();// 唯一Id
            registration.Name = _consulOptions.Name; // 服务名(分组--多个实例组成的集群)
            registration.Address = _consulOptions.Address; // 服务绑定IP
            registration.Port = Convert.ToInt32(_consulOptions.Port); // 服务绑定端口
                                                                //Tag 标签
            registration.Check = new AgentServiceCheck
            {
                
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // 服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10), // 健康检查时间间隔
                HTTP = $"http://{_consulOptions.Ip}:{_consulOptions.Port}{_consulOptions.HealthCheck}", // 健康检查地址
                Timeout = TimeSpan.FromSeconds(5) // 超时时间

            };
            await client.Agent.ServiceRegister(registration);
        }
    }
}
