using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.MultiFreeSql.Options
{
    public class ConnectOptions
    {
        public string DbKey { get; set; }

        public string DbType { get; set; }

        public string MasterConnection { get; set; }

        public List<string> SlaveConnections { get; set; }

        public bool AutoSyncStructure { get; set; }

    }
}
