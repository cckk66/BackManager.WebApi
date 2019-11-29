using System.Collections.Generic;
using System.Threading.Tasks;
using BackManager.Utility;
using BackManager.Utility.Common;
using BackManager.WebApi.Signal.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Redis;

namespace BackManager.WebApi.Signal
{
    /// <summary>
    /// 系统通讯类
    /// </summary>
    public class SysHub : BaseHub<SignalSysUser>
    {
        private readonly IServiceStackRedisCache _serviceStackRedisCache;
        public SysHub(IServiceStackRedisCache serviceStackRedisCache)
        {
            _serviceStackRedisCache = serviceStackRedisCache;
            this.MyConnectedBefore += SysHub_MyConnectedBefore;
        }

        private SignalSysUser SysHub_MyConnectedBefore(Microsoft.AspNetCore.SignalR.HubCallerContext hubCallerContext, SignalSysUser signalSysUser)
        {

            signalSysUser.UserID = System.Convert.ToInt64(hubCallerContext.GetHttpContext().Request.Query["userId"]);
            return signalSysUser;
        }

        /// <summary>
        /// 用户集合
        /// </summary>
        protected override List<SignalSysUser> SignalOnLineUsers
        {
            get
            {
                return _serviceStackRedisCache.Get<List<SignalSysUser>>(RedisCacheKey.SysHubOnlineUserKey);
            }
            set
            {
                _serviceStackRedisCache.Set<List<SignalSysUser>>(RedisCacheKey.SysHubOnlineUserKey, value);
            }
        }

        /// <summary>
        /// 获取广播消息
        /// </summary>
        /// <param name="UserID">请求用户ID</param>
        /// <returns></returns>
        public async Task SysMessage(long UserID)
        {

            await Clients.All.SendAsync("SysMessage", new List<SignalSysUser> { });
        }
        /// <summary>
        /// 指定客户端发送
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SendUserID"></param>
        /// <param name="signalSysUser"></param>
        /// <returns></returns>
        public async Task SendClientByUser(long UserID, long SendUserID, SignalSysUser signalSysUser)
        {
            string ConnectionId = this.FindSignalUser(m => m.UserID == SendUserID)?.ConnectionId;
            await Clients.Client(ConnectionId).SendAsync("userMessageReceived", signalSysUser);
        }

    }
}
