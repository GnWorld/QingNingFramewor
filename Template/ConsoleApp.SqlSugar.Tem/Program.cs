using ConsoleApp.SqlSugar.Tem;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiDbSqlSugar;


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



HostApplicationBuilder? builder = Host.CreateApplicationBuilder();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddSqlSugar(builder.Configuration);
builder.Services.AddTransient<TestService>();
builder.Services.AddHostFiltering(o => { });

IHost? host = builder.Build();

var testService = host.Services.GetRequiredService<TestService>();
await testService.Test();

await host.RunAsync();