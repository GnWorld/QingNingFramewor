using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace QingNing.MultiDbSqlSugar;

/// <summary>
/// SqlSugar 事务和工作单元
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{

    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
    {
        this._sqlSugarClient = sqlSugarClient;
        _logger = logger;

    }
    public void BeginTransaction(ActionExecutingContext context)
    {
        _sqlSugarClient.AsTenant().BeginTran();
    }

    public void CommitTransaction(ActionExecutedContext resultContext)
    {
        _sqlSugarClient.AsTenant().CommitTran();
    }

    public void OnCompleted(ActionExecutingContext context, ActionExecutedContext resultContext)
    {
        _sqlSugarClient.Dispose();
    }

    public void RollbackTransaction(ActionExecutedContext resultContext)
    {
        _sqlSugarClient.AsTenant().RollbackTran();
    }
}