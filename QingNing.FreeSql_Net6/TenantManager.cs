using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.FreeSql_Net6
{
    public class TenantManager
    {
        // 注意一定是 static 静态化
        static AsyncLocal<long> _asyncLocal = new AsyncLocal<long>();
        public static long Current
        {
            get => _asyncLocal.Value;
            set => _asyncLocal.Value = value;
        }
    }
}
