using System;

namespace BackManager.Domain
{
    /// <summary>
    /// SysGroup
    /// </summary>
    public class SysGroup : BasicBusinessAggregateRoot
    {

		


		/// <summary>
		/// 组名称
		/// </summary>
		public string GroupName { get; set; }


		/// <summary>
		/// 组类型 备用
		/// </summary>
		public int GroupType { get; set; }


		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedAt { get; set; }


		/// <summary>
		/// 创建人
		/// </summary>
		public long CreatedUserId { get; set; }


		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdatedAt { get; set; }


		/// <summary>
		/// 更新人
		/// </summary>
		public long? UpdatedUserId { get; set; }



		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; }


    }
}
