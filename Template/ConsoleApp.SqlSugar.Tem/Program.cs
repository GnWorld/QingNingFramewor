using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using ConsoleApp.SqlSugar.Tem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.AOP;
using QingNing.MultiDbSqlSugar.UOW;

using QingNing.Internal;
//WebApplicationBuilder webBuilder = WebApplication.CreateBuilder();
//await webBuilder.Build().RunAsync();
//var configuration = webBuilder.Configuration;

//configuration.AddJsonFile("appsettings.json");
////服务
//var services = webBuilder.Services;
//services.AddSqlSugar(configuration);
//services.AddTransient<TestService>();
//WebApplication? webhost = webBuilder.Build();
//webhost.Run();


Microsoft.Extensions.Configuration.IConfiguration Configuration;

var builder = Host.CreateDefaultBuilder();
builder.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(o =>
{
    //注入拦截器
    o.RegisterType<UnitOfWorkInterceptor>();
    //注入工作单元管理类
    o.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>();
    
    //注入TestService
    o.RegisterType<TestService>()
        .AsImplementedInterfaces()  //映射为接口  这里注入的 TestService 将会 自动注入ITestService
        .EnableInterfaceInterceptors()  // 开启接口拦截   在使用ITestService时 会自动拦截  ，注意直接使用TestService 不会拦截; 也可以通过  EnableClassInterceptors() 开启实现方法的拦截
        .InterceptedBy(typeof(UnitOfWorkInterceptor)); //配置拦截器  可 同时配置多个 ，例如  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())
});


builder.ConfigureHostConfiguration(o =>
{
    o.AddJsonFile("appsettings.json");
    o.Build().ConfigureHostConfiguration();
})

.ConfigureServices(services =>
{
    services.AddSqlSugar();
    services.AddTransient<TestService>();
});


IHost? host = builder.Build();

var testService = host.Services.GetRequiredService<ITestService>();
await testService.Test();

await host.RunAsync();