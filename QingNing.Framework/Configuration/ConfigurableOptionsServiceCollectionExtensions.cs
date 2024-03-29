﻿using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QingNing.Configuration
{
    public static class ConfigurableOptionsServiceCollectionExtensions
    {
        /// <summary>
        /// 添加选项配置
        /// </summary>
        /// <typeparam name="TOptions">选项类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddConfigurableOptions<TOptions>(this IServiceCollection services)
            where TOptions : class, IOption
        {

            return services;
        }
        public static void AddConfigurationJsonFile(this IConfigurationBuilder configuration, string path = "Configurations")
        {
            var jsonSettings = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path), "*.json");
            for (int i = 0; i < jsonSettings.Length; i++)
            {
                configuration.AddJsonFile(jsonSettings[i]);
            }
        }

    }
}
