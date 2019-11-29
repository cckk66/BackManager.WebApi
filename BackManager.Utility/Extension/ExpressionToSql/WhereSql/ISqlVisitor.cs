using BackManager.Utility.Extension.ExpressionJoin;
using BackManager.Utility.Tool;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BackManager.Utility.Extension.ExpressionToSql.Sql
{
    public enum SqlType
    {
        SqlServer = 1,
        MySql = 2
    }
    public interface ISqlVisitor
    {
      
        SqlResult WhereSql<T>(Expression<Func<T, bool>> expression);

    };
    public class SqlVisitorFactory
    {
        private static ISqlVisitor _SqlServerSingleton;
        private static ISqlVisitor _MySqlSingleton;
        private SqlVisitorFactory() { }

        public static ISqlVisitor GetSingleton(SqlType sqlType)
        {
            switch (sqlType)
            {
                case SqlType.SqlServer:
                    if (_SqlServerSingleton == null)
                    {
                        _SqlServerSingleton = new SqlServerVisitor();
                    }
                    return _SqlServerSingleton;
                default:
                    if (_MySqlSingleton == null)
                    {
                        _MySqlSingleton = new MySqlVisitor();
                    }
                    return _MySqlSingleton;
            }
        }

    }
}
