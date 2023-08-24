using ConsoleApp.SqlSugar.Tem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiDbSqlSugar;


Host.CreateDefaultBuilder()
    .ConfigureHostConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json");
    })
    .ConfigureServices(services =>
    {

        services.AddSqlSugar(configuration);
        services.AddTransient<TestService>();
    }).Build().Run();


