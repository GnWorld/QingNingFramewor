using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.FreeSql_Net6
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public record DbConnectionOptionsConfig
    {
        public string MasterConnetion { get; set; }
        public List<string> SlaveConnections { get; set; }
    }
}
