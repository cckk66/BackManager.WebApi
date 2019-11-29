using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
namespace BackManager.Utility.Extension
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientUserIp(this HttpContext context)
        {
            try
            {
                string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Connection.RemoteIpAddress.ToString();
                }
                return ip;
            }
            catch (Exception)
            {

                return "获取ip错误";
            }

        }
        /// <summary>
        /// 主机名称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetHostName(this HttpContext context)
        {
            try
            {
                string username = System.Net.Dns.GetHostEntry(context.Request.HttpContext.Connection.RemoteIpAddress).HostName;
                return username;
            }
            catch (Exception)
            {

                return "获取主机错误";
            }

        }
    }
}
