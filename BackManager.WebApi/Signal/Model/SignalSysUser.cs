﻿using BackManager.Utility;

namespace BackManager.WebApi.Signal.Model
{

    /// <summary>
    /// 系统通讯消息
    /// </summary>
    public class SignalSysUser : SignalUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserID { get; set; }

    }
}
