// See https://aka.ms/new-console-template for more information
using ConsoleApp.FreeSqlTemplate.Data;
using ConsoleApp.FreeSqlTemplate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

var builder = Host.CreateApplicationBuilder();
builder.Logging.AddSerilog(Log.Logger);

//加载appsetting.json 配置文件
builder.Configuration.AddJsonFile("appsettings.json");

//注入FreeSql
builder.Services.RegisterFreeSql(builder.Configuration);


//注入仓储
builder.Services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);

//注入Service
builder.Services.AddTransient<IdsSyncUserService>();

var app = builder.Build();

var _syncUserService = app.Services.GetRequiredService<IdsSyncUserService>();
await _syncUserService.SyncUser();

app.Run();

