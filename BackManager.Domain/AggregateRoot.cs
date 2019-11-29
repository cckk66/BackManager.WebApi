using System;

namespace BackManager.Domain
{
    public class AggregateRoot : AggregateRoot<long>, IAggregateRoot
    {

    }
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
    /// <summary>
    /// 基础业务聚合 常规增删改查
    /// </summary>
    public class BasicBusinessAggregateRoot : AggregateRoot, IDeleted, ICreated, IUpdated
    {
        public int DeleteFlag { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeleteUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedUserId { get;set;}
    }
}