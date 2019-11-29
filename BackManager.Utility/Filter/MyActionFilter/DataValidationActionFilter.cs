using BackManager.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace BackManager.Utility.Filter
{

    /// <summary>
    /// 数据验证过滤器
    /// </summary>
    public class DataValidationActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var ModelValues = context.ModelState.Values;
                context.Result = new OkObjectResult(ApiResult<string>.Error(""));

            }
            //if (context.ActionArguments != null && context.ActionArguments.Count > 0)
            //{
            //    foreach (KeyValuePair<string, object> actionArgument in context.ActionArguments)
            //    {
            //        if (actionArgument.Value != null && actionArgument.Value.GetType().IsClass)
            //        {

            //            IEnumerable<PropertyInfo> props = actionArgument.Value.GetType()
            //                .GetProperties(System.Reflection.BindingFlags.Public)
            //                .Where(m => m.GetCustomAttribute<ValidationAttribute>(true) != null);
            //            foreach (PropertyInfo prop in props)
            //            {

            //            }

            //        }
            //    }
            //}
            await next();

            //await context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }
}
