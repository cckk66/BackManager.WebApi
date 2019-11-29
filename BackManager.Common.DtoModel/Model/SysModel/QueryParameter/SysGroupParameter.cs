namespace BackManager.Common.DtoModel.Model
{
    /// <summary>
    /// 用户分组查询
    /// </summary>
    public class SysGroupParameter
    {
        public string GroupName { get; set; }
        public long UserID { get; set; }
        public string Remark { get; set; }
        public int DeleteFlag { get; set; }
    }
}
