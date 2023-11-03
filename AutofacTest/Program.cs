using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacTest;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(o =>
    {
        o.RegisterModule<AutofacModuleRegister>();

    }).ConfigureServices(services => {

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    });





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
