using Castle.DynamicProxy;

namespace AutofacTest;

public class TestInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine("方法名" + invocation.Method?.Name);
    }
}
