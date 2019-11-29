using BackManager.Utility.Tool;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BackManager.Utility.Extension.ExpressionToSql.Sql
{
    internal class MySqlVisitor : ISqlVisitor
    {
        public SqlResult WhereSql<T>(Expression<Func<T, bool>> expression)
        {
            return expression.ToParMySql();
        }
    }
}