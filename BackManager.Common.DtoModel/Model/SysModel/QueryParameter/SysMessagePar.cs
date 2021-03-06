﻿using System;

namespace BackManager.Common.DtoModel.Model.SysModel.QueryParameter
{
    public class SysMessagePar
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        public DateTime? PutStartDate { get; set; }
        public DateTime? PutEndDate { get; set; }
        public int DeleteFlag { get; set; }
    }
}
