using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BackManager.Utility
{
    public class SignalUser
    {
        public string ConnectionId { get; set; }
        public DateTime LoginDate { get; set; } = new DateTime();
        public ClaimsPrincipal User { get; set; }
        /// <summary>
        /// 终止时错错误信息
        /// </summary>
        public Exception exception { get; set; }
    };
}
