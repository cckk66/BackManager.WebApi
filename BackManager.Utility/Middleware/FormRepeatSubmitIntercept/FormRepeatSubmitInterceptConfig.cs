using Microsoft.Extensions.Options;

namespace BackManager.Utility.Middleware
{
    /// <summary>
    /// 表单重复提交配置
    /// </summary>
    public class FormRepeatSubmitInterceptConfig : IOptions<FormRepeatSubmitInterceptConfig>
    {
        /// <summary>
        /// 表单唯一标识
        /// </summary>
        public string FormUniqueIdentification { get; set; }

        /// <summary>
        /// 设置表单重复提示返回信息
        /// </summary>
        public string FormRepeatSubmitReturnValue{ get; set; }

        public FormRepeatSubmitInterceptConfig Value => this;
    }
}
