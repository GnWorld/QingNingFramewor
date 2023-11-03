using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using MultiSugarTestApi;
using QingNing.MultiDbSqlSugar;
using QingNing.MultiDbSqlSugar.AOP;
using QingNing.MultiDbSqlSugar.UOW;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(o =>
    {
        //ע��������
        o.RegisterType<UnitOfWorkAOP>();
        //ע�빤����Ԫ������
        o.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>();

        //ע��TestService
        o.RegisterType<TestService>()
            .AsImplementedInterfaces()  //ӳ��Ϊ�ӿ�  ����ע��� TestService ���� �Զ�ע��ITestService
            .EnableInterfaceInterceptors()  // �����ӿ�����   ��ʹ��ITestServiceʱ ���Զ�����  ��ע��ֱ��ʹ��TestService ��������; Ҳ����ͨ��  EnableClassInterceptors() ����ʵ�ַ���������
            .InterceptedBy(typeof(UnitOfWorkAOP)); //����������  �� ͬʱ���ö�� ������  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())


    });
builder.Services.AddSqlSugar();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
