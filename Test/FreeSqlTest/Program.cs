
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

})
    

.UseServiceProviderFactory(new AutofacServiceProviderFactory())


.ConfigureContainer<ContainerBuilder>(container =>
{
    //拦截器
    container.RegisterType<UnitOfWorkInterceptor>();

    //工作单元管理
    container.RegisterType<UnitOfWorkManager>().InstancePerLifetimeScope();

    //注入TestService
    container.RegisterType<TestService>()
        .AsImplementedInterfaces() 
        .EnableInterfaceInterceptors()  
        .InterceptedBy(typeof(UnitOfWorkInterceptor));

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