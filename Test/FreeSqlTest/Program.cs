
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Castle.Core.Configuration;
using FreeSql;
using FreeSqlTest;
using FreeSqlTest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiFreeSql.Base;
using QingNing.MultiFreeSql.Uow;
using Serilog;
using Serilog.Events;


//日志
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

var configBuilder = new ConfigurationBuilder();




var builder = Host.CreateDefaultBuilder();

builder.ConfigureLogging(loggin =>
{
    loggin.AddSerilog(Log.Logger);

}).UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(container =>
{
    //注入拦截器
    container.RegisterType<UnitOfWorkInterceptor>();
    //注入工作单元管理类
    container.RegisterType<UnitOfWorkManager>().InstancePerLifetimeScope();

    //注入TestService
    container.RegisterType<TestService>()
        .AsImplementedInterfaces()  //映射为接口  这里注入的 TestService 将会 自动注入ITestService
        .EnableInterfaceInterceptors()  // 开启接口拦截   在使用ITestService时 会自动拦截  ，注意直接使用TestService 不会拦截; 也可以通过  EnableClassInterceptors() 开启实现方法的拦截
        .InterceptedBy(typeof(UnitOfWorkInterceptor)); //配置拦截器  可 同时配置多个 ，例如  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())

}).ConfigureHostConfiguration(x =>
{
    configBuilder.AddJsonFile("appsettings.json");
    x = configBuilder;
})
.ConfigureServices(services =>
{

    services.RegisterFreeSql(configBuilder.Build());
    services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);

});

var app = builder.Build();

var _testServices = app.Services.GetRequiredService<ITestService>();
await _testServices.Test();

app.Run();