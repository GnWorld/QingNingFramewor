namespace QingNing.Consul
{
    public class ConsulOptions
    {
        /// <summary>
        /// Consul 客户端地址
        /// </summary>
        public string ConsulIp { get; set; }

        public int ConsulPort { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务绑定Ip
        /// </summary>
        public string ServiceIp { get; set; }

        /// <summary>
        /// 服务绑定端口
        /// </summary>
        public string ServicePort { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }



    }
}
