using QingNing.Consul.ConsulExtends;
using Consul;
using QingNing.Consul;
using TestWebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("Configurations\\Consul.json");
// Add services to the container.
builder.Services.Configure<ConsulOptions>(builder.Configuration.GetSection("ConsulOptions"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע��Consul
builder.Services.AddConsulRegister();
builder.Services.AddFreeSQLConfiguration(builder.Configuration);


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