using System;
using System.Collections.Generic;
using System.Text;
using BackManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork,IDisposable where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction _DbContextTransaction { get; set; } = null;

        public UnitOfWork(TDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void BginTran()
        {
            if (_DbContextTransaction != null)
                _DbContextTransaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_DbContextTransaction != null)
                _DbContextTransaction.Commit();
        }

        public void Rollback()
        {
            if (_DbContextTransaction != null)
                _DbContextTransaction.Rollback();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_DbContextTransaction != null)
                _DbContextTransaction.Dispose();
        }
    }

}
