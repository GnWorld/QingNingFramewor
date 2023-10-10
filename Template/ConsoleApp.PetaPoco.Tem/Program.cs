// See https://aka.ms/new-console-template for more information

using ConsoleApp.PetaPoco.Tem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetaPoco.Core;
using System.Data.Common;


var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddJsonFile("appsettings.json");
var connectString = builder.Configuration.GetSection("ConnectionStrings:MysqlConnectString").Value;
var providerName = builder.Configuration.GetSection("ConnectionStrings:ProviderName").Value;

var context = new AipNaipDbContext(connectString,providerName );

builder.Services.AddSingleton<AipNaipDbContext>(context);
builder.Services.AddTransient<TestService>();

var host = builder.Build();

var testService = host.Services.GetRequiredService<TestService>();

await testService.Test();

Console.WriteLine("Hello, World!");
