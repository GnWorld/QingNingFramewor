using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using MultiSugarTestApi;
using QingNing.Internal;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.AOP;
using QingNing.MultiDbSqlSugar.UOW;
using QingNing.Swagger;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(o =>
    {
        //注入拦截器
        o.RegisterType<UnitOfWorkInterceptor>();
        //注入工作单元管理类
        o.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>();

        //注入TestService
        o.RegisterType<TestService>()
            .AsImplementedInterfaces()  //映射为接口  这里注入的 TestService 将会 自动注入ITestService
            .EnableInterfaceInterceptors()  // 开启接口拦截   在使用ITestService时 会自动拦截  ，注意直接使用TestService 不会拦截; 也可以通过  EnableClassInterceptors() 开启实现方法的拦截
            .InterceptedBy(typeof(UnitOfWorkInterceptor)); //配置拦截器  可 同时配置多个 ，例如  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())


    });
builder.ConfigureApplication();
builder.Services.AddSqlSugar();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSetup();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSession();
//    app.UseSwaggerAuthorized();
//    app.UseSwaggerMiddle(() => Assembly.GetExecutingAssembly().GetManifestResourceStream("index.html"));
//}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
