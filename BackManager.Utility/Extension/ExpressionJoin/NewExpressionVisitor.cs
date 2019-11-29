using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BackManager.Utility.Extension
{
    /// <summary>
    /// 建立新表达式
    /// </summary>
    public class MyExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression _NewParameter { get; private set; }
        public MyExpressionVisitor(ParameterExpression param)
        {
            this._NewParameter = param;
        }
        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this._NewParameter;
        }
    }
}
