using QingNing.Consul;
using QingNing.Consul.ConsulExtends;
using QingNing.MultiDbSqlSugar;
using QingNing.Framework;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigurationJsonFile("Configurations");

// Add services to the container.
builder.Services.Configure<ConsulOptions>(builder.Configuration.GetSection("ConsulOptions"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//×¢ÈëConsul
builder.Services.AddConsulRegister();

builder.Services.AddSqlSugar(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseConsulRegister();

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
