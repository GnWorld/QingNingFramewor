using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QingNing.MultiDbSqlSugar.AOP;
using QingNing.MultiDbSqlSugar.UOW;
using Serilog;
using System.Reflection;

namespace QingNing.MultiDbSqlSugar.Extensions
{
    public static class DbExtensions
    {
        public static string GetFullName(this MethodInfo method)
        {
            if (method.DeclaringType == null)
            {
                return $@"{method.Name}";
            }

            return $"{method.DeclaringType.FullName}.{method.Name}";
        }


        /// <summary>
        /// 判断类型是否实现某个泛型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="generic">泛型类型</param>
        /// <returns>bool</returns>
        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            // 检查接口类型
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 检查类型
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            return false;

            // 判断逻辑
            bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        }


        public static IHostBuilder ConfigureHost(this IHostBuilder hostBuilder, string serviceDllName)
        {
            hostBuilder.ConfigureContainer<ContainerBuilder>(o =>
             {
                 //注入拦截器
                 o.RegisterType<UnitOfWorkInterceptor>();
                 //注入工作单元管理类
                 o.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>();

                 var basePath = AppContext.BaseDirectory;
                 //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

                 var servicesDllFile = Path.Combine(basePath, serviceDllName);
                 if (!File.Exists(servicesDllFile))
                 {
                     var msg = "service.dll 丢失";
                     Log.Error(msg);
                     throw new Exception(msg);
                 }
                 var assemblysServices = Assembly.LoadFrom(servicesDllFile);
                 o.RegisterAssemblyTypes(assemblysServices)
                     .AsImplementedInterfaces()  //映射为接口  这里注入的 TestService 将会 自动注入ITestService
                     .InstancePerLifetimeScope()
                     .PropertiesAutowired()
                     .EnableClassInterceptors()
                     .EnableInterfaceInterceptors()    // 开启接口拦截   在使用ITestService时 会自动拦截  ，注意直接使用TestService 不会拦截; 也可以通过  EnableClassInterceptors() 开启实现方法的拦截
                     .InterceptedBy(typeof(UnitOfWorkInterceptor)); //配置拦截器  可 同时配置多个 ，例如  var aops  = List<Type>();    aops.Add(typeof(UnitOfWorkAOP));     .InterceptedBy(aops.ToArray())
             });
            return hostBuilder;

        }
    }


}
