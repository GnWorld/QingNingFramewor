using QingNing.Consul;
using QingNing.Consul.ConsulExtends;
using QingNing.MultiDbSqlSugar;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Configurations\\Consul.json");
builder.Configuration.AddJsonFile("Configurations\\DbSettings.json");
// Add services to the container.
builder.Services.Configure<ConsulOptions>(builder.Configuration.GetSection("ConsulOptions"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//×¢ÈëConsul
builder.Services.AddConsulRegister();

//builder.Services.AddFreeSQLConfiguration(builder.Configuration);

builder.Services.AddSqlSugar(builder.Configuration);

var app = builder.Build();
app.Services.GetService<IConsulRegister>()?.ConsulRegistAsync().Wait();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthCheckMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
