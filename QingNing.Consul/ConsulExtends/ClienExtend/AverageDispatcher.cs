using Microsoft.Extensions.Options;
using System;

namespace QingNing.Consul
{
    /// <summary>
    /// 平均
    /// </summary>
    public class AverageDispatcher : AbstractConsulDispatcher
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

        public AverageDispatcher(IOptionsMonitor<ConsulOptions> consulClientOption) : base(consulClientOption)
        {
        }
        #endregion

        /// <summary>
        /// 平均
        /// </summary>
        /// <returns></returns>
        protected override int GetIndex()
        {
            return new Random(iTotalCount++).Next(0, _CurrentAgentServiceDictionary.Length);
        }
    }
}
