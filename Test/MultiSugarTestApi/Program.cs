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
        //ע��������
        o.RegisterType<UnitOfWorkInterceptor>();
        //ע�빤����Ԫ������
        o.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>();

        //ע��TestService
        o.RegisterType<TestService>()
            .AsImplementedInterfaces()  //ӳ��Ϊ�ӿ�  ����ע��� TestService ���� �Զ�ע��ITestService
            .EnableInterfaceInterceptors()  // �����ӿ�����   ��ʹ��ITestServiceʱ ���Զ�����  ��ע��ֱ��ʹ��TestService ��������; Ҳ����ͨ��  EnableClassInterceptors() ����ʵ�ַ���������
            .InterceptedBy(typeof(UnitOfWorkInterceptor)); //����������  �� ͬʱ���ö�� ������  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())


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
