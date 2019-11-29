using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BackManager.Domain.Model.Sys
{
    public class ExceptionLog : AggregateRoot
    {
        public long UserID { get; set; }

        public string Message { get; set; }
        public string InnerException { get; set; }
        public string OpMethod { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ESouceType SouceType { get; set; } = ESouceType.Backstage;
    }
    public enum ESouceType
    {
        /// <summary>
        /// 后台
        /// </summary>
        [Description("后台")]
        Backstage = 1
    }
}
