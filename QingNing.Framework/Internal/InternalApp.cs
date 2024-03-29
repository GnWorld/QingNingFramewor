﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QingNing.Internal;
public static class InternalApp
{

    internal static IServiceCollection InternalServices;

    /// <summary>根服务</summary>
    internal static IServiceProvider RootServices;

    /// <summary>获取Web主机环境</summary>
    internal static IWebHostEnvironment WebHostEnvironment;

    /// <summary>获取泛型主机环境</summary>
    internal static IHostEnvironment HostEnvironment;

    /// <summary>配置对象</summary>
    internal static IConfiguration Configuration;

    public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder wab)
    {
        HostEnvironment = wab.Environment;
        WebHostEnvironment = wab.Environment;
        InternalServices = wab.Services;
        Configuration = wab.Configuration;
        return wab;
    }

    public static void ConfigureHostConfiguration(this IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static void ConfigureApplication(this IHost app)
    {
        RootServices = app.Services;
    }
}
