using Castle.DynamicProxy;

namespace MultiSugarTestApi;

public class MyInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine("调用方法前");
        invocation.Proceed();

        Console.WriteLine("调用方法后");

    }
}
