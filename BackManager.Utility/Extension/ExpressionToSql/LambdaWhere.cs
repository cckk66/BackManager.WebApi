using System;
using System.Linq.Expressions;

namespace BackManager.Utility.Extension.ExpressionToSql
{
    /// <summary>
    /// where条件简单评价
    /// </summary>
    public static class LambdaWhere
    {
        private enum JoinType
        {
            And = 1,
            Or = 2
        }
        public static Expression<Func<Par, bool>> WhereIFAnd<Par>(this Expression<Func<Par, bool>> lambda1, bool ifWhere, Expression<Func<Par, bool>> lambda2)
            where Par : class
        {
            lambda1 = Join(JoinType.And, lambda1, ifWhere, lambda2);
            return lambda1;
        }
        public static Expression<Func<Par, bool>> WhereIFOr<Par>(this Expression<Func<Par, bool>> lambda1, bool ifWhere, Expression<Func<Par, bool>> lambda2)
            where Par : class
        {
            lambda1 = Join(JoinType.Or, lambda1, ifWhere, lambda2);
            return lambda1;
        }
        private static Expression<Func<Par, bool>> Join<Par>(JoinType joinType, Expression<Func<Par, bool>> lambda1, bool ifWhere, Expression<Func<Par, bool>> lambda2) where Par : class
        {
            if (lambda1 == null)
            {
                if (ifWhere)
                {
                    lambda1 = lambda2;
                }
            }
            else
            {
                if (ifWhere)
                {
                    switch (joinType)
                    {

                        case JoinType.Or:
                            lambda1 = lambda1.Or(lambda2);
                            break;
                        default:
                            lambda1 = lambda1.And(lambda2);
                            break;
                    }

                }
            }

            return lambda1;
        }
    }
}
