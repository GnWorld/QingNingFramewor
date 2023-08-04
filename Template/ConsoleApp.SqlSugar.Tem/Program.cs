using ConsoleApp.SqlSugar.Tem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiDbSqlSugar;
using SqlSugar;

var build = Host.CreateApplicationBuilder();
build.Configuration.AddJsonFile("appsettings.json");

build.Services.AddSqlSugar(build.Configuration);
build.Services.AddTransient<TestService>();


var app = build.Build();
var _testService = app.Services.GetService(typeof(TestService)) as TestService;
await _testService.Test();

app.Run();