using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
//using System.Data.SqlClient; The SqlParameterCollection only accepts non-null SqlParameter type objects, not SqlParameter objects.
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using BackManager.Domain;
using BackManager.Utility;
using BackManager.Utility.Extension;
using BackManager.Utility.Extension.ExpressionToSql;
using Microsoft.EntityFrameworkCore;
using UnitOfWork;

namespace BackManager.Infrastructure
{
    public class EfCoreRepository<TEntity>
        : EfCoreRepository<TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity, IAggregateRoot
    {
        public EfCoreRepository(UnitOfWorkDbContext dbDbContext) : base(dbDbContext)
        {
        }
    }

    public class EfCoreRepository<TEntity, TPrimaryKey>
        : Repository<TEntity, TPrimaryKey>, IRepositoryWithDbContext
        where TEntity : class, IEntity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        private readonly UnitOfWorkDbContext _dbContext;

        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public EfCoreRepository(UnitOfWorkDbContext dbDbContext)
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
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (DbCommand cmd = Connection.CreateCommand())
                {
                    //cmd.Connection = Connection;
                    if (parameters != null)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            object obj = parameters[i];
                            if (obj != null)
                            {
                                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                                {
                                    SqlParameter sqlParameter = new SqlParameter(propertyInfo.Name, propertyInfo.GetValue(obj));
                                    cmd.Parameters.Add(sqlParameter);
                                }
                            }
                        }
                    }
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    using (DbDataReader dbDataReader = cmd.ExecuteReader())
                    {
                        return dbDataReader.ToList<Dto>();
                    }


                }
            }

        }


        public override IList<Dto> GetAlls<Dto>(string sql, List<ParResult> parResults)
        {
            //_dbContext.Set<Dto>().FromSql
            var Connection = _dbContext.Database.GetDbConnection();
            //using (var Connection = _dbContext.Database.GetDbConnection())
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (DbCommand cmd = Connection.CreateCommand())
                {
                    //cmd.Connection = Connection;
                    SetParameter(parResults, cmd);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    using (DbDataReader dbDataReader = cmd.ExecuteReader())
                    {
                        return dbDataReader.ToList<Dto>();
                    }


                }
            }

        }

        private static void SetParameter(List<ParResult> parResults, DbCommand cmd)
        {
            if (parResults != null)
            {
                parResults.ForEach(par =>
                {
                    SqlParameter sqlParameter = new SqlParameter(par.Key, par.Value);
                    cmd.Parameters.Add(sqlParameter);
                });

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
        public override int ExecuteScalar(string sql)
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
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
            }
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
            _dbContext.Entry(entity).Property(propertyExpression).IsModified = true;
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
    }
}