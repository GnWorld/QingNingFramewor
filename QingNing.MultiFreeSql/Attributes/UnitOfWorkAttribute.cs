using FreeSql;

namespace QingNing.MultiFreeSql.Attributes;

public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 事务传播方式
    /// </summary>
    public Propagation Propagation { get; set; } = Propagation.Required;
}
