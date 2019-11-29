using BackManager.Domain;
using BackManager.Utility.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BackManager.Utility.Filter.FormRepeatSubmitIntercept
{
    /// <summary>
    /// 资源过滤器
    /// </summary>
    public class FormRepeatSubmitInterceptFilter : IAsyncResourceFilter
    {
        private readonly IOptions<FormRepeatSubmitInterceptConfig> _optionsAccessor;
        private readonly IServiceStackRedisCache _serviceStackRedisCache;

        public FormRepeatSubmitInterceptFilter(
            IOptions<FormRepeatSubmitInterceptConfig> optionsAccessor
            , IServiceStackRedisCache serviceStackRedisCache)
        {
            _optionsAccessor = optionsAccessor;
            _serviceStackRedisCache = serviceStackRedisCache;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            HttpContext httpContext = context.HttpContext;
            bool isFormRepeatSubmit = false;

            var bm = httpContext.Request.Headers[_optionsAccessor.Value.FormUniqueIdentification].ToString();
            if (!string.IsNullOrEmpty(bm))
            {
                try
                {
                    {
                        //验证是否重复提交

                        if (_serviceStackRedisCache.StringIncrement(bm) != 1)
                        {
                            isFormRepeatSubmit = true;
                            //存在重复提交返回
                            context.Result = new OkObjectResult(string.IsNullOrWhiteSpace(_optionsAccessor.Value.FormRepeatSubmitReturnValue) ? "" : _optionsAccessor.Value.FormRepeatSubmitReturnValue);
                        }
                    }
                    if (!isFormRepeatSubmit)
                    {
                        await next();
                    }
                }
                finally
                {
                    //释放表单
                    _serviceStackRedisCache.StringDecrement(bm);
                }
            }
            else
            {
                await next();
            }

        }
    }
}
