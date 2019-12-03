using System;

namespace BackManager.Common.DtoModel.Model.SysModel
{
    public class SysMessageDto
    {
        public long ID { get; set; }
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
        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreatedUserName { get; set; }


        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdatedUserName { get; set; }
    }
}
