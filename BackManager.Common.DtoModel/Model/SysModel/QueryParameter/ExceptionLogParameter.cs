using System;

namespace BackManager.Common.DtoModel.Model.SysModel
{
    /// <summary>
    /// 错误日志参数
    /// </summary>
    public class ExceptionLogParameter
    {
        public string Dates { get; set; } = ",";
        public DateTime CreatedAt { get; set; }
        public object SouceType { get; set; }
    }
}
