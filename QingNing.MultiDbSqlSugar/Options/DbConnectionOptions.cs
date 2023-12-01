using SqlSugar;

namespace QingNing.MultiDbSqlSugar;

/// <summary>
/// 数据库配置选项
/// </summary>
public sealed class DbConnectionOptions
{
    /// <summary>
    /// 数据库集合
    /// </summary>
    public List<DbConnectionConfig> ConnectionConfigs { get; set; }

}

public sealed class DbConnectionConfig : ConnectionConfig
{
    /// <summary>
    /// 启用库表初始化
    /// </summary>
    public bool EnableInitDb { get; set; }

    /// <summary>
    /// 启用种子初始化
    /// </summary>
    public bool EnableInitSeed { get; set; }

    /// <summary>
    /// 启用库表差异日志
    /// </summary>
    public bool EnableDiffLog { get; set; }

    /// <summary>
    /// 启用驼峰转下划线
    /// </summary>
    public bool EnableUnderLine { get; set; }

    /// <summary>
    /// 从库
    /// </summary>
    public List<SlaveConnectionConfig> SlaveConnectionConfigs { get; set; }


}