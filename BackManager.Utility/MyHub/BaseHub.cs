using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackManager.Utility
{
    public abstract class BaseHub<T> : Hub
         where T : SignalUser, new()
    {
        /// <summary>
        /// 用户集合
        /// </summary>
        /// <returns></returns>
        protected abstract List<T> SignalOnLineUsers { get; set; }
        /// <summary>
        /// 建立新连接时之前终止之前委托,用于用户自定义扩展 ,传入登录用户Model, 返回登录用户Model
        /// </summary>
        protected event Func<HubCallerContext, T, T> MyConnectedBefore;

        /// <summary>
        /// 建立新连接时
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnMyConnectedAsync(HubCallerContext hubCallerContext)
        {
            return Task.Run(() =>
            {
                if (hubCallerContext != null)
                {
                    T signalUser = new T
                    {
                        ConnectionId = hubCallerContext.ConnectionId,
                        User = hubCallerContext.User
                    };

                    if (MyConnectedBefore != null)
                    {
                        signalUser = MyConnectedBefore(hubCallerContext, signalUser);
                    }
                    SignalOnLineUsers.Add(signalUser);
                }
            });
        }


        public override Task OnConnectedAsync()
        {
            this.OnMyConnectedAsync(Context);
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 集线器的连接终止之前委托,用于用户自定义扩展 
        /// </summary>
        protected event Action<HubCallerContext> DisconnectedBefore;

        /// <summary>
        /// 集线器的连接终止之前
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual Task OnDisconnectedAsync(HubCallerContext hubCallerContext)
        {
            return Task.Run(() =>
            {
                if (DisconnectedBefore != null)
                {
                    DisconnectedBefore(hubCallerContext);
                }
                SignalOnLineUsers = SignalOnLineUsers.Where(m => m.ConnectionId != hubCallerContext.ConnectionId).ToList();
            });
        }
        /// <summary>
        /// 集线器的连接终止
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            this.OnDisconnectedAsync(Context);
            return base.OnDisconnectedAsync(exception);
        }
        /// <summary>
        /// 获取指定登录用户
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        protected T FindSignalUser(Func<T, bool> predicate)
        {
            if (this.SignalOnLineUsers != null)
            {
                return this.SignalOnLineUsers.Where(predicate).FirstOrDefault();

            }
            return null;
        }

    }
}
