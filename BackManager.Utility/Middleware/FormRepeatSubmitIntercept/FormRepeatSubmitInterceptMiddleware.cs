using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BackManager.Utility.Middleware
{
    /// <summary>
    /// 表单重复提交中间件
    /// </summary>
    public class FormRepeatSubmitInterceptMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<FormRepeatSubmitInterceptConfig> _optionsAccessor;
        private readonly IServiceStackRedisCache _serviceStackRedisCache;

        public FormRepeatSubmitInterceptMiddleware(RequestDelegate next
            , IOptions<FormRepeatSubmitInterceptConfig> optionsAccessor
            , IServiceStackRedisCache serviceStackRedisCache)
        {
            _next = next;
            _optionsAccessor = optionsAccessor;
            _serviceStackRedisCache = serviceStackRedisCache;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            #region 读取body
            //{
            //    httpContext.Request.EnableBuffering();
            //    var requestReader = new StreamReader(httpContext.Request.Body);

            //    var requestContent = await requestReader.ReadToEndAsync();
            //    Console.WriteLine($"Request Body: {requestContent}");
            //    httpContext.Request.Body.Position = 0;
            //}
            #endregion

            bool isFormRepeatSubmit = false;


            var bm = httpContext.Request.Headers[_optionsAccessor.Value.FormUniqueIdentification].ToString();
            if (!string.IsNullOrEmpty(bm))
            {

                {
                    //验证是否重复提交

                    if (_serviceStackRedisCache.StringIncrement(bm) != 1)
                    {
                        isFormRepeatSubmit = true;
                        //存在重复提交返回
                        await httpContext.Response.WriteAsync("当前表单重复提交");
                    }
                }
                if (!isFormRepeatSubmit)
                {

                    Stream originalBody = httpContext.Response.Body;
                    try
                    {
                        using (var memStream = new MemoryStream())
                        {
                            httpContext.Response.Body = memStream; //httpContext.Response 不允许读取 将可读取Stream替换body中Stream
                            await _next(httpContext);
                            memStream.Position = 0;
                            //读取body 内容
                            string responseBody = new StreamReader(memStream).ReadToEnd();
                            memStream.Position = 0;
                            await memStream.CopyToAsync(originalBody);
                        }

                    }
                    finally
                    {
                        httpContext.Response.Body = originalBody;
                        //释放表单
                        _serviceStackRedisCache.StringDecrement(bm);

                    }


                }
            }
            else
            {
                await _next(httpContext);
            }

        }
    }
}