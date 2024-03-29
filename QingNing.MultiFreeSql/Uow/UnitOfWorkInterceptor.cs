﻿using Castle.DynamicProxy;
using FreeSql;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Relational;
using QingNing.MultiFreeSql.Attributes;
using System.Reflection;

namespace QingNing.MultiFreeSql.Uow;
public class UnitOfWorkInterceptor : IInterceptor
{
    private readonly ILogger<UnitOfWorkInterceptor> _logger;
    private readonly UnitOfWorkManager _uowManager;

    public UnitOfWorkInterceptor(ILogger<UnitOfWorkInterceptor> logger, UnitOfWorkManager uowManager)
    {
        _logger = logger;
        _uowManager = uowManager;
    }

    public void Intercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        if (method.GetCustomAttribute<UnitOfWorkAttribute>(true) is { } uta)
        {
            try
            {
                using var uow = _uowManager.Begin(uta.Propagation);

                try
                {
                    invocation.Proceed();

                    if (IsAsyncMethod(method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }
                    uow.Commit();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());

                throw;
            }
        }
        else
        {
            invocation.Proceed(); //直接执行被拦截方法
        }

    }
    public static bool IsAsyncMethod(MethodInfo method)
    {
        return (
            method.ReturnType == typeof(Task) ||
            (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        );
    }
}
