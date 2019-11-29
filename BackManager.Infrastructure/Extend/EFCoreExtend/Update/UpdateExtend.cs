using BackManager.Utility.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Infrastructure
{


    public class MySqlBulkUpdate<TEntity>
    {

        private List<TEntity> _UpdateData;
        private dynamic _WherePropertyInfo;
        private DbContext _dbContext;

        public MySqlBulkUpdate(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public MySqlBulkUpdate<TEntity> Update(List<TEntity> UpdateData)
        {
            _UpdateData = UpdateData;
            return this;
        }

        //PropertyEntry<TEntity, TProperty> Property<TProperty>([NotNullAttribute] Expression<Func<TEntity, TProperty>> propertyExpression)
        /// <summary>
        /// 与临时表拼接条件
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="WherePropertyInfo"></param>
        /// <returns></returns>
        public MySqlBulkUpdate<TEntity> WhereJoin<TProperty>(Expression<Func<TEntity, TProperty>> WherePropertyInfo)
        {

            _WherePropertyInfo = WherePropertyInfo;
            return this;
        }
        public async Task<int> SetAsync<TProperty>(Expression<Func<TEntity, TProperty>> SetPropertyInfo)
        {
            
            List<string> SetFields = new List<string>();
            List<string> WhereOns = new List<string>();
            {
                string SetField = SetPropertyInfo.PropertyCondition();
                foreach (string field in SetField.Split(','))
                {
                    SetFields.Add($"T.{field}=Temp.{field}");
                }
            }
            {
                ExpressionTPropertyExend.tPropertyVisitor.Visit(_WherePropertyInfo);
                string WhereOn = ExpressionTPropertyExend.tPropertyVisitor.Condition();
                foreach (string field in WhereOn.Split(','))
                {
                    WhereOns.Add($"T.{field}=Temp.{field}");
                }
            }
            var Connection = _dbContext.Database.GetDbConnection();
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                string TabelName = typeof(TEntity).Name;
                string TempTabelName = $@"Temp{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                using (DbCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = $"CREATE TEMPORARY TABLE {TempTabelName}(SELECT * from {TabelName} where 1=2);";
                    cmd.ExecuteNonQuery();

                    DataTable dt = new DataTable("Table");
                    dt = _UpdateData.ToDataTable();
                    if (await _dbContext.MySqlBulkInsertByStreamAsync(TempTabelName, dt) > 0)
                    {
                        // Updating destination table, and dropping temp table

                        cmd.CommandTimeout = 300;

                        cmd.CommandText = $@"UPDATE 
                                                {TabelName} T INNER JOIN {TempTabelName} temp on {string.Join(",", WhereOns)}
                                             SET  {string.Join(",", SetFields)};
                                             DROP TABLE {TempTabelName};";
                       return await cmd.ExecuteNonQueryAsync();
                    }



                }
            }
            return 0;
        }



    }



}
