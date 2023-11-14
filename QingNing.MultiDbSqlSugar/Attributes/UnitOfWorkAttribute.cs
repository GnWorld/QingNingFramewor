using Castle.DynamicProxy;
using System.Reflection;

namespace QingNing.MultiDbSqlSugar.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 事务传播方式
    /// </summary>
    public Propagation Propagation { get; set; } = Propagation.Required;
}
