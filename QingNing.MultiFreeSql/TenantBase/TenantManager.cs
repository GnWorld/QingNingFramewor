namespace QingNing.MultiFreeSql;
/// <summary>
/// 多租户管理器
/// </summary>
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
