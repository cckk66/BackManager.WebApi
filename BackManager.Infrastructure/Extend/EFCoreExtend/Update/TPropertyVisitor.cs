using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BackManager.Infrastructure
{
    /// <summary>
    /// Property生成
    /// </summary>
    public class TPropertyVisitor : ExpressionVisitor
    {
        protected Stack<string> _StringStack = new Stack<string>();
        protected bool IsBinaryOperator = false;
        public string Condition()
        {
            string condition = string.Join(",", this._StringStack.ToArray());
            this._StringStack.Clear();
            return condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException("MemberExpression");
            this._StringStack.Push(node.Member.Name);
            return node;
        }

    }
}
