namespace BackManager.Domain
{
    /// <summary>
    /// 主键
    /// </summary>
    public class PrimaryKey : IPrimaryKey
    {
        public long ID { get; set; }
    }
    public interface IPrimaryKey: IEntity<long>
    {
    }
}
