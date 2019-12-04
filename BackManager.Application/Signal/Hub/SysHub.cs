using System.Collections.Generic;
using System.Threading.Tasks;
using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Utility;
using BackManager.Utility.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Redis;

namespace BackManager.Application
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
        protected override List<SignalSysUser> GetSignalOnLineUsers()
        {
            List<SignalSysUser> signalSysUsers = _serviceStackRedisCache.Get<List<SignalSysUser>>(RedisCacheKey.SysHubOnlineUserKey);
            if (signalSysUsers == null)
            {
                signalSysUsers = new List<SignalSysUser>();
            }
            return signalSysUsers;
        }

        protected override void SetSignalOnLineUsers(List<SignalSysUser> ts)
        {
            //string str = Newtonsoft.Json.JsonConvert.SerializeObject(ts);
            //_serviceStackRedisCache.StringSet(RedisCacheKey.SysHubOnlineUserKey, str);
            _serviceStackRedisCache.Set<List<SignalSysUser>>(RedisCacheKey.SysHubOnlineUserKey, ts);
        }


   
        /// <summary>
        /// 指定客户端发送
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SendUserID"></param>
        /// <param name="signalSysUser"></param>
        /// <returns></returns>
        public async Task SendClientByUser(long UserID, long SendUserID, SysMessageDto sysMessageDto)
        {
            string ConnectionId = this.FindSignalUser(m => m.UserID == SendUserID)?.ConnectionId;
            await Clients.Client(ConnectionId).SendAsync("userMessageReceived", sysMessageDto);
        }

       
    }
}
