using System;

namespace BackManager.Common.DtoModel.Model.SysModel
{
    public class SysMessageDto
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
