using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.Attributes;
using SqlSugar;

namespace MultiSugarTestApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;


    private readonly TestService _testService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, TestService testService)
    {
        _logger = logger;
        _testService = testService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        await _testService.Test();


        return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray());


    }
}


[SugarTable]
public class AppRole
{
    /// <summary>
    /// ½ÇÉ«ID
    /// </summary>
    [JsonProperty, SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// ½ÇÉ«Ãû³Æ
    /// </summary>
    [JsonProperty, SugarColumn(ColumnName = "role_name", ColumnDataType = "varchar(60)")]
    public string RoleName { get; set; } = string.Empty;

    [SugarColumn(IsNullable = false)]
    public string Code { get; set; }
}


