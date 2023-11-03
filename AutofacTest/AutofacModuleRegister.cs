using Autofac;

namespace AutofacTest;

public class AutofacModuleRegister:Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TestInterceptor>();
    }
}
