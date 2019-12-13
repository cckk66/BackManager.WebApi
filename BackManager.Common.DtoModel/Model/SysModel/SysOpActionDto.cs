using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Common.DtoModel.Model
{
    public class SysOpActionDto
    {
        public long ID { get; set; }
        // <summary>
        /// 按钮名
        /// </summary>
        public string ActionName { get; set; }


        /// <summary>
        /// 按钮图标
        /// </summary>
        public string ActionIcon { get; set; }


        /// <summary>
        /// 按钮的页面ID
        /// </summary>
        public string ActionButtonID { get; set; }
    }
}
