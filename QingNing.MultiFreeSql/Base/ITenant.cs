using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingNing.MultiFreeSql.Base;

/// <summary>
/// 多租户
/// </summary>
public interface ITenant
{
    public long TenantId { get; set; }
}
