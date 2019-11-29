using BackManager.Domain;
using BackManager.Domain.Model;
using BackManager.Domain.Model.Sys;
using BackManager.Infrastructure.MyEFLogger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UnitOfWork
{
    public class UnitOfWorkDbContext : DbContext
    {
        public UnitOfWorkDbContext(DbContextOptions<UnitOfWorkDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            {
                var loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(new EFLoggerProvider());
                optionsBuilder.UseLoggerFactory(loggerFactory);
            }
        }
        public DbSet<SysGroup> SysGroups { get; set; }
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<SysUserGroup> SysUserGroups { get; set; }
        public DbSet<SysMenu> SysMenus { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<SysMessage> SysMessages { get; set; }
        public DbSet<DTTest> DTTests { get; set; }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysGroup>()
                .ToTable("SysGroup")
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .ToTable("SysUser")
                .Property(e => e.LoginName)
                .IsUnicode(false);

            modelBuilder.Entity<SysUserGroup>().ToTable("SysUserGroup");
            modelBuilder.Entity<SysMenu>().ToTable("SysMenu");
            modelBuilder.Entity<ExceptionLog>().ToTable("ExceptionLog");
            modelBuilder.Entity <SysMessage>().ToTable("SysMessage");
            modelBuilder.Entity<DTTest>().ToTable("DTTest");


        }
    }
}