using ConsoleApp.FreeSqlTemplate.Data.Models;
using System.Linq.Expressions;

namespace ConsoleApp.FreeSqlTemplate.Data.Repository;
/// <summary>
/// 用户仓储
/// </summary>
public class FPLUserRepository : BaseRepositoryExtend<FPLUser, Guid>
{
    public FPLUserRepository(FreeSqlCloud cloud, Expression<Func<FPLUser, bool>> filter = null, Func<string, string> asTable = null) : base(cloud.Use(DbEnum.FPL), filter, asTable)
    {

    }
}
