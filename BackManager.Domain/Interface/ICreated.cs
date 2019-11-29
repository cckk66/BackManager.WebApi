using System;

namespace BackManager.Domain
{
    /// <summary>
    /// 创建信息接口
    /// </summary>
    public interface ICreated
    {
        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Desc:创建用户
        /// Default:
        /// Nullable:False
        /// </summary>           
        long CreatedUserId { get; set; }
    }
}
