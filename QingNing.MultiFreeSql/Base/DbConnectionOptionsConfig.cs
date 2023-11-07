using FreeSql;

namespace QingNing.MultiFreeSql.Base;

/// <summary>
/// 数据库连接配置
/// </summary>
public record DbConnectionOptionsConfig
{

    /// <summary>
    /// 数据库类型
    /// </summary>
    public DataType DataType { get; set; }

    /// <summary>
    /// 主库连接
    /// </summary>
    public string MasterConnetion { get; set; }
    /// <summary>
    /// 从库连接
    /// </summary>
    public List<string> SlaveConnections { get; set; }
}
