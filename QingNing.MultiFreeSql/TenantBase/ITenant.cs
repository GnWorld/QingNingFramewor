namespace QingNing.MultiFreeSql;
public interface ITenant
{
    /// <summary>
    /// 多租户Id
    /// </summary>
    public long TenantId { get; set; }
}
