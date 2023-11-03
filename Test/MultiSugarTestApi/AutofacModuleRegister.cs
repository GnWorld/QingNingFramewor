using Autofac;
using Autofac.Extras.DynamicProxy;
using QingNing.MultiDbSqlSugar.AOP;
using QingNing.MultiDbSqlSugar.UOW;
using System.Reflection;

namespace MultiSugarTestApi;

public class AutofacModuleRegister : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var basePath = AppContext.BaseDirectory;
        var servicesDllFile = Path.Combine(basePath, "MultiSugarTestApi.dll");
        var cacheType = new List<Type>();
        builder.RegisterType<UnitOfWorkAOP>();
        cacheType.Add(typeof(UnitOfWorkAOP));
        builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>)).InstancePerDependency();
        var assemblysServices = Assembly.LoadFrom(servicesDllFile);
        builder.RegisterAssemblyTypes(assemblysServices)
    .AsImplementedInterfaces()
    .InstancePerDependency()
    .PropertiesAutowired()
    .EnableInterfaceInterceptors()       //引用Autofac.Extras.DynamicProxy;
    .InterceptedBy(cacheType.ToArray()); //允许将拦截器服务的列表分配给注册。

        builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
    .AsImplementedInterfaces()
    .InstancePerLifetimeScope()
    .PropertiesAutowired();
    }
}
