using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackManager.Utility.Middleware
{
    /// <summary>
    /// 表单重复提交拦截配置
    /// </summary>
    public static class FormRepeatSubmitInterceptExtensions
    {
        public static IServiceCollection AddFormRepeatSubmitIntercept(this IServiceCollection services, Action<FormRepeatSubmitInterceptConfig> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);

            return services;
        }

        public static IApplicationBuilder UseFormRepeatSubmitIntercept(this IApplicationBuilder app)
        {
            app.UseMiddleware<FormRepeatSubmitInterceptMiddleware>();
            return app;
        }
    }
}
