using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackManager.Infrastructure
{
    public static class MySqlBulkUpdateExtend
    {
        /// <summary>
        /// 批量修改 生成临时表 update a from a b where a.id=b.id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="UpdateData"></param>
        /// <returns></returns>
        public static MySqlBulkUpdate<TEntity> BulkUpdate<TEntity>(this DbContext dbContext, List<TEntity> UpdateData)
        {
            return new MySqlBulkUpdate<TEntity>(dbContext).Update(UpdateData);
        }
    }
}
