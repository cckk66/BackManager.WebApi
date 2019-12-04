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
        /// 获取用户集合
        /// </summary>
        /// <returns></returns>
        protected abstract List<T> GetSignalOnLineUsers();
        /// <summary>
        /// 设置获取用户集合
        /// </summary>
        /// <returns></returns>
        protected abstract void SetSignalOnLineUsers(List<T> ts);
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
                        //Identity = hubCallerContext.User.Identity
                    };
                    List<T> newSignalOnLineUsers = AutoMapperHelper.MapToList<T, T>(this.GetSignalOnLineUsers()).ToList();
                    if (MyConnectedBefore != null)
                    {
                        signalUser = MyConnectedBefore(hubCallerContext, signalUser);
                    }

                    newSignalOnLineUsers.Add(signalUser);
                    this.SetSignalOnLineUsers(newSignalOnLineUsers);
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
                List<T> newSignalOnLineUsers = AutoMapperHelper.MapToList<T, T>(this.GetSignalOnLineUsers()).ToList();
                newSignalOnLineUsers = newSignalOnLineUsers.Where(m => m.ConnectionId != hubCallerContext.ConnectionId).ToList();
                this.SetSignalOnLineUsers(newSignalOnLineUsers);
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
            List<T> newSignalOnLineUsers = this.GetSignalOnLineUsers();
            if (newSignalOnLineUsers != null)
            {
                return newSignalOnLineUsers.Where(predicate).FirstOrDefault();

            }
            return null;
        }

    }
}
