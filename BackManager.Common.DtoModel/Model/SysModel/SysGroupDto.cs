using BackManager.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackManager.Common.DtoModel
{
    public class SysGroupDto : IValidatableObject
    {
        public long ID { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.GroupName))
            {
                yield return new ValidationResult(Newtonsoft.Json.JsonConvert.SerializeObject(ApiResult<string>.Error("分组名称不能为空")));
            }
        }


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
