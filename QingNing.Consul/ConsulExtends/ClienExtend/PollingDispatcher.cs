﻿using Microsoft.Extensions.Options;

namespace QingNing.Consul
{
    /// <summary>
    /// 轮询
    /// </summary>
    public class PollingDispatcher : AbstractConsulDispatcher
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

        public PollingDispatcher(IOptionsMonitor<ConsulOptions> consulClientOption) : base(consulClientOption)
        {
        }
        #endregion

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="serviceCount"></param>
        /// <returns></returns>
        protected override int GetIndex()
        {
            return iTotalCount++ % _CurrentAgentServiceDictionary.Length;
        }
    }
}
