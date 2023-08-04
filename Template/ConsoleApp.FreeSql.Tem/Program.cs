// See https://aka.ms/new-console-template for more information
using ConsoleApp.FreeSqlTemplate.Data;
using ConsoleApp.FreeSqlTemplate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.RegisterFreeSql(builder.Configuration);
builder.Services.AddFreeRepository(null, typeof(BaseRepositoryExtend<,>).Assembly);
builder.Services.AddTransient<TestServices>();

var app = builder.Build();
var testService = app.Services.GetService<TestServices>();
await testService.TestAsync();


app.Run();

