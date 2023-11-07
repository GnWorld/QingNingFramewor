using QingNing.MultiFreeSql.TenantBase;

namespace QingNing.MultiFreeSql.EntityBase;
public class TenantEntity : Entity, ITenant
{
    public long TenantId { get; set; }
}
