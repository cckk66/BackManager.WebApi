using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Utility.MySecretInsuranceCard
{
    public static class SecretInsuranceCardExtensions
    {
        public static IServiceCollection AddSecretInsuranceCard(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            /*AddTransient瞬时模式：每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例

            AddScoped：每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例

            AddSingleton单例模式：每次都获取同一个实例
            */
            services.AddScoped<ISecretInsuranceCard, SecretInsuranceCard>();
            return services;
        }

        public static IServiceCollection AddSecretInsuranceCard<T>(this IServiceCollection services)
          where T : class, ISecretInsuranceCard
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            /*AddTransient瞬时模式：每次请求，都获取一个新的实例。即使同一个请求获取多次也会是不同的实例

            AddScoped：每次请求，都获取一个新的实例。同一个请求获取多次会得到相同的实例

            AddSingleton单例模式：每次都获取同一个实例
            */
            services.AddScoped<ISecretInsuranceCard, T>();
            return services;
        }
    }
}
