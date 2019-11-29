using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackManager.Utility.Tool.Swagger
{
    /// </summary>
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
      
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                //非匿名的方法,链接中添加accesstoken值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "BmForm",
                        In = ParameterLocation.Header,
                        Required = false //是否必选
                    });
                }
            }
        }
    }
}