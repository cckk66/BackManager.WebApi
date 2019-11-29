using System;
using System.Linq.Expressions;

namespace BackManager.Infrastructure
{
    public static class ExpressionTPropertyExend
    {
        private static TPropertyVisitor _tPropertyVisitor;
        public static TPropertyVisitor tPropertyVisitor
        {
            get
            {
                if (_tPropertyVisitor == null)
                {
                    _tPropertyVisitor = new TPropertyVisitor();
                }
                return _tPropertyVisitor;

            }
        }

        public static string PropertyCondition<TEntity, TProperty>(this Expression<Func<TEntity, TProperty>> _lambda)
        {
            if (_lambda == null) return "";
            tPropertyVisitor.Visit(_lambda);
            return tPropertyVisitor.Condition();
        }
    }
}
