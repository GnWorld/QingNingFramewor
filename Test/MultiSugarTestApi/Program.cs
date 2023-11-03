using Autofac;
using Autofac.Extensions.DependencyInjection;
using MultiSugarTestApi;
using QingNing.MultiDbSqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(o =>
    {
        o.RegisterModule<AutofacModuleRegister>();
        o.RegisterType<TestService>().AsSelf();
        o.RegisterModule<AutofacPropertityModuleReg>();

    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlSugar(builder.Configuration);
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

app.Run();
