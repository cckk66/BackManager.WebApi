using System;

namespace BackManager.Domain
{
    /// <summary>
    /// 修改基类
    /// </summary>
    public interface IUpdated
    {
        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Desc:创建用户
        /// Default:
        /// Nullable:False
        /// </summary>           
        long? UpdatedUserId { get; set; }
    }
}
