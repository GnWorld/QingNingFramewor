using Consul;
using Microsoft.Extensions.Options;

namespace QingNing.Consul
{
    public abstract class AbstractConsulDispatcher
    {
        protected ConsulOptions _consulOptions;
        protected KeyValuePair<string, AgentService>[] _CurrentAgentServiceDictionary;

        public AbstractConsulDispatcher(IOptionsMonitor<ConsulOptions> consulClientOption)
        {
            _consulOptions = consulClientOption.CurrentValue;
        }

        /// <summary>
        /// 负载均衡获取地址
        /// </summary>
        /// <param name="mappingUrl">Consul映射后的地址</param>
        /// <returns></returns>
        public string GetAddress(string mappingUrl)
        {
            Uri uri = new Uri(mappingUrl);
            string serviceName = uri.Host;
            string addressPort = ChooseAddress(serviceName);
            return $"{uri.Scheme}://localhost:{addressPort}{uri.PathAndQuery}";
        }

        protected virtual string ChooseAddress(string serviceName)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://localhost:8500/");
            });
            AgentService agentService = null;

            var response = client.Agent.Services().Result.Response;//获取Consul全部服务清单
            var dictionary = response.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToArray();
            var entrys = client.Health.Service(serviceName).Result.Response;
            List<KeyValuePair<string, AgentService>> serviceList = new List<KeyValuePair<string, AgentService>>();
            for (int i = 0; i < entrys.Length; i++)
            {
                serviceList.Add(new KeyValuePair<string, AgentService>(i.ToString(), entrys[i].Service));
            }
            _CurrentAgentServiceDictionary = serviceList.ToArray();

            int index = GetIndex();
            agentService = _CurrentAgentServiceDictionary[index].Value;

            return $"{agentService.Port}";
        }

        protected abstract int GetIndex();
    }
}
