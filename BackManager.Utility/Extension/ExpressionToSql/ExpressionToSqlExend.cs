using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BackManager.Utility.Tool
{
    public static class ExpressionToSqlExend
    {
        private static ConditionBuilderVisitor _conditionBuilderVisitor;
        private static ConditionBuilderVisitor conditionBuilderVisitor
        {
            get
            {
                if (_conditionBuilderVisitor == null)

                {
                    _conditionBuilderVisitor = new ConditionBuilderVisitor();
                }
                return _conditionBuilderVisitor;

            }
        }

        public static string ToSql<T>(this Expression<Func<T, bool>> _lambda)
        {
            if (_lambda == null) return "";
            conditionBuilderVisitor.Visit(_lambda);
            string where= conditionBuilderVisitor.Condition();
            return string.IsNullOrEmpty(where) ? "" : " where " + where;
        }
    }

    public static class ExpressionToParSqlExend
    {
        private static ConditionParBuilderVisitor _conditionBuilderVisitor;
        private static ConditionParBuilderVisitor conditionBuilderVisitor
        {
            get
            {
                if (_conditionBuilderVisitor == null)

                {
                    _conditionBuilderVisitor = new ConditionParBuilderVisitor();
                }
                return _conditionBuilderVisitor;

            }
        }

        public static SqlResult ToParSql<T>(this Expression<Func<T, bool>> _lambda)
        {
            if (_lambda == null) return new SqlResult();
            conditionBuilderVisitor.Visit(_lambda);
            SqlResult sqlResult = conditionBuilderVisitor.ParCondition();
            return sqlResult;


        }
        private static ConditionMySqlParBuilderVisitor _conditionMySqlParBuilderVisitor;
        private static ConditionMySqlParBuilderVisitor conditionMySqlParBuilderVisitor
        {
            get
            {
                if (_conditionMySqlParBuilderVisitor == null)

                {
                    _conditionMySqlParBuilderVisitor = new ConditionMySqlParBuilderVisitor();
                }
                return _conditionMySqlParBuilderVisitor;

            }
        }

        public static SqlResult ToParMySql<T>(this Expression<Func<T, bool>> _lambda)
        {
            if (_lambda == null) return new SqlResult();
            conditionMySqlParBuilderVisitor.Visit(_lambda);
            SqlResult sqlResult = conditionMySqlParBuilderVisitor.ParCondition();
            return sqlResult;


        }
        
    }
}
