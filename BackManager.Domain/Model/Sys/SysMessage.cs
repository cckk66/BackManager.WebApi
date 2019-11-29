using System;

namespace BackManager.Domain.Model.Sys
{
    /// <summary>
    ///系统消息
    /// </summary>
    public class SysMessage : BasicBusinessAggregateRoot
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        public DateTime PutStartDate { get; set; } = DateTime.Now;
        public DateTime PutEndDate { get; set; } = DateTime.Now;

    }
}
