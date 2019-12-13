namespace BackManager.Domain.Model.Sys
{
    /// <summary>
    /// 菜单按钮
    /// </summary>
    public class SysOpAction : BasicBusinessAggregateRoot
    {

        /// <summary>
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
