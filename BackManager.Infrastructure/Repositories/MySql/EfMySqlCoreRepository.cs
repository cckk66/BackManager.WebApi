using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackManager.Domain;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MySql.Data.MySqlClient;
using UnitOfWork;

namespace BackManager.Infrastructure
{
    public class EfMySqlCoreRepository<TEntity>
        : EfMySqlCoreRepository<TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity, IAggregateRoot
    {
        public EfMySqlCoreRepository(UnitOfWorkDbContext dbDbContext) : base(dbDbContext)
        {
        }
    }

    public class EfMySqlCoreRepository<TEntity, TPrimaryKey>
        : MySqlRepository<TEntity, TPrimaryKey>, IRepositoryWithDbContext
        where TEntity : class, IEntity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        private readonly UnitOfWorkDbContext _dbContext;

        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public EfMySqlCoreRepository(UnitOfWorkDbContext dbDbContext)
        {
            _dbContext = dbDbContext;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }
        public override IQueryable<TEntity> GetAll(string sql, params object[] parameters)
        {
            return Table.FromSqlRaw(sql, parameters);
        }
        public override IList<Dto> GetAlls<Dto>(string sql, params object[] parameters) => this.GetAlls<Dto>(sql, CommandType.Text, parameters);
        public override IList<Dto> GetAlls<Dto>(string sql, CommandType commandType, params object[] parameters)
        {
            var Connection = _dbContext.Database.GetDbConnection();

            //using (var Connection = _dbContext.Database.GetDbConnection())
            //{
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            using (DbCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandType = commandType;
                cmd.CommandText = sql;
                DbDataReader dbDataReader = cmd.ExecuteReader();
                return dbDataReader.ToList<Dto>();


            }
            //}

        }
        public override IList<Dto> GetAlls<Dto>(string sql, List<ParResult> parResults)
        {
            var Connection = _dbContext.Database.GetDbConnection();
            //using (var Connection = _dbContext.Database.GetDbConnection())
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (DbCommand cmd = Connection.CreateCommand())
                {
                    SetParameter(parResults, cmd);
                    cmd.CommandType = CommandType.Text;
                    //MySqlCommand
                    cmd.CommandText = sql;
                    try
                    {
                        using (var dbDataReader = cmd.ExecuteReader())
                        {
                            return dbDataReader.ToList<Dto>();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return null;


                }
            }

        }
        public override int ExecuteScalar(string sql, List<ParResult> parResults)
        {
            var Connection = _dbContext.Database.GetDbConnection();
            //using (var Connection = _dbContext.Database.GetDbConnection())
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                using (DbCommand cmd = Connection.CreateCommand())
                {
                    SetParameter(parResults, cmd);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
            }
        }
        private static void SetParameter(List<ParResult> parResults, DbCommand cmd)
        {
            if (parResults != null)
            {
                parResults.ForEach(par =>
                {
                    MySqlParameter sqlParameter = new MySqlParameter(par.Key, par.Value);
                    cmd.Parameters.Add(sqlParameter);
                });

            }
        }

        public override int ExecuteScalar(string sql)
        {
            var Connection = _dbContext.Database.GetDbConnection();
            //using (var Connection = _dbContext.Database.GetDbConnection())
            //{
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            using (DbCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
            // }
        }


        public override TEntity Insert(TEntity entity)
        {
            var newEntity = Table.Add(entity).Entity;
            //_dbContext.SaveChanges();
            return newEntity;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            //_dbContext.SaveChanges();

            return entity;
        }
        public override TEntity Update<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            AttachIfNot(entity);
            try
            {
                string SetField = propertyExpression.PropertyCondition();
                EntityEntry<TEntity> entityEntry = _dbContext.Entry(entity);
                foreach (string field in SetField.Split(','))
                {
                    entityEntry.Property(field).IsModified = true;
                }
                   
            }
            catch (Exception ex)
            {

            }
            return entity;
        }
        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);

            //_dbContext.SaveChanges();
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }
        /// <summary>
        ///   Attach方法：将给定实体以 System.Data.EntityState.Unchanged 状态附加到上下文中
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _dbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = _dbContext.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, ((TEntity)ent.Entity).ID)
                );

            return entry?.Entity as TEntity;
        }
        #region Bulk 操作
        public override Task<int> BulkInsert(List<TEntity> entities)
        {
            return _dbContext.MySqlBulkInsert<TEntity>(entities);
        }


        public override Task<int> BulkUpdate(List<TEntity> Entities,
            Expression<Func<TEntity, dynamic>> SetProperty,
            Expression<Func<TEntity, dynamic>> JoinProperty)
        {
            return _dbContext.BulkUpdate(Entities).WhereJoin(JoinProperty)
                      .SetAsync(SetProperty);

        }

        #endregion
    }
}