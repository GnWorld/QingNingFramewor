using Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var client = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "my-service-2",
    Name = "My Service",
    Address = "host.docker.internal",
    Port = 5119,
    Tags = new[] { "my-tag" },
    Check = new AgentServiceCheck()
    {

        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(10),
        Interval = TimeSpan.FromSeconds(10),
        HTTP = "http://host.docker.internal:5119/health",
        Timeout = TimeSpan.FromSeconds(5),


    }

};


//client.Agent.ServiceRegister(registration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => "OK");

app.Run();
