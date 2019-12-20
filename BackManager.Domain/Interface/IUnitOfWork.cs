namespace BackManager.Domain
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        void BginTran();
        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
        int SaveChanges();
    }
}
