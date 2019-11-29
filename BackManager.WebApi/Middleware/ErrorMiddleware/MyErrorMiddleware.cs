using BackManager.Application;
using BackManager.Domain;
using BackManager.Utility.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace BackManager.Utility.Middleware.ErrorMiddleware
{
    /// <summary>
    /// 错误管道中间件
    /// </summary>
    public class MyErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly IExceptionLogService _exceptionLogService;
        /// <summary>
        /// DI,注入logger和环境变量
        /// </summary>
        /// <param name="next"></param>
        /// <param name="env"></param>
        /// <param name="exceptionLogService"></param>
        public MyErrorMiddleware(RequestDelegate next, IWebHostEnvironment env, IExceptionLogService exceptionLogService)
        {
            _next = next;
            _env = env;
            _exceptionLogService = exceptionLogService;
        }
        /// <summary>
        /// 实现Invoke方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleError(context, ex);
            }
        }
        /// <summary>
        /// 错误信息处理方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/json;charset=utf-8;";
            string errorMsg = $@"错误消息:{ex.Message}{Environment.NewLine}详细错误:{ex.InnerException?.Message}{Environment.NewLine}错误追踪:{ex.StackTrace}";
            //无论是否为开发环境都记录错误日志
            await _exceptionLogService.InsertAsync(new BackManager.Common.DtoModel.ExceptionLogDto()
            {
                IP = context.GetClientUserIp(),
                CreatedAt = DateTime.Now,
                Message = ex.Message,
                InnerException = errorMsg,
                OpMethod = context.Request.Path,
                UserID = -1
            });
            //浏览器在开发环境显示详细错误信息,其他环境隐藏错误信息
            string ErrorMessage = "";
            if (_env.IsDevelopment())
            {
                ErrorMessage = errorMsg;
            }
            else
            {
                ErrorMessage = "抱歉，服务端出错了";
            }

            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(ApiResult<string>.Error(ErrorMessage)));
        }
    }
}
