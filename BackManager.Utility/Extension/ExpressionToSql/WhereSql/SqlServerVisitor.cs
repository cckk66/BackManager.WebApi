
using BackManager.Utility.Tool;
using System;
using System.Linq.Expressions;

namespace BackManager.Utility.Extension.ExpressionToSql.Sql
{
    internal class SqlServerVisitor : ISqlVisitor
    {

        public SqlResult WhereSql<T>(Expression<Func<T, bool>> expression)
        {
            return expression.ToParSql();
        }
    }
}