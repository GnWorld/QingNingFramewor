// See https://aka.ms/new-console-template for more information
using ConsoleApp.FreeSqlTemplate.Data;
using ConsoleApp.FreeSqlTemplate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

var builder = Host.CreateApplicationBuilder();
builder.Logging.AddSerilog(Log.Logger);
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.RegisterFreeSql(builder.Configuration);
builder.Services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);
builder.Services.AddTransient<TestServices>();
builder.Services.AddTransient<IdsSyncUserService>();

var app = builder.Build();
//var testService = app.Services.GetService<TestServices>();
//await testService.TestAsync();
var _syncUserService = app.Services.GetService<IdsSyncUserService>();
await _syncUserService.SyncUser();

app.Run();

