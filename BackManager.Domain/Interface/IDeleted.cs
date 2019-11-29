using System;

namespace BackManager.Domain
{
    /// <summary>
    /// 删除基类
    /// </summary>
    public interface IDeleted
    {
        
        int DeleteFlag { get; set; }
        /// <summary>
        /// Desc:删除时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Desc:删除用户
        /// Default:
        /// Nullable:True
        /// </summary>           
         long? DeleteUserId { get; set; }
    }
}
