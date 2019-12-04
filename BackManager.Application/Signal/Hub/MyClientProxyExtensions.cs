using BackManager.Common.DtoModel.Model.SysModel;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BackManager.Application.Signal
{
    public static class MyClientProxyExtensions
    {
        /// <summary>
        /// 获取广播消息
        /// </summary>
        /// <param name="UserID">请求用户ID</param>
        /// <returns></returns>
        public static async Task SysMessage(this IHubClients @Clients, SysMessageDto sysMessageDto)
        {
            await @Clients.All.SendAsync("sysMessage", sysMessageDto.Title, sysMessageDto.Content);
        }


        /// <summary>
        /// 指定客户端发送
        /// </summary>
        /// <param name="UserID">请求用户ID</param>
        /// <returns></returns>
        public static async Task SendClientByUser(this IHubClients @Clients, string ConnectionId, SysMessageDto sysMessageDto)
        {
            await Clients.Client(ConnectionId).SendAsync("userMessageReceived", sysMessageDto);
        }
    }
}
