﻿using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QingNing.Consul
{
    /// <summary>
    /// 权重
    /// </summary>
    public class WeightDispatcher : AbstractConsulDispatcher
    {
        #region Identity
        private static int _iTotalCount = 0;
        private static int iTotalCount
        {
            get
            {
                return _iTotalCount;
            }
            set
            {
                _iTotalCount = value >= int.MaxValue ? 0 : value;
            }
        }
        public WeightDispatcher(IOptionsMonitor<ConsulOptions> consulClientOption) : base(consulClientOption)
        {

        }
        #endregion

        protected override string ChooseAddress(string serviceName)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"{_consulOptions.Address}");
            });
            AgentService agentService = null;
            var response = client.Agent.Services().Result.Response;

            _CurrentAgentServiceDictionary = response.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToArray();


            var serviceDictionaryNew = new List<AgentService>();
            foreach (var service in _CurrentAgentServiceDictionary)
            {
                serviceDictionaryNew.AddRange(Enumerable.Repeat(service.Value, int.TryParse(service.Value.Tags?[0], out int iWeight) ? 1 : iWeight));
            }
            int index = new Random(DateTime.Now.Millisecond).Next(0, int.MaxValue) % serviceDictionaryNew.Count;
            agentService = serviceDictionaryNew[index];

            return $"{agentService.Address}:{agentService.Port}";
        }
        /// <summary>
        /// 不需要了
        /// </summary>
        /// <returns></returns>
        protected override int GetIndex()
        {
            throw new NotImplementedException();
        }
    }
}
