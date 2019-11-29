using BackManager.Utility.Tool.ExpressionToSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BackManager.Utility.Tool
{

    /// <summary>
    /// 条件生成
    /// </summary>
    public class ConditionBuilderVisitor : ExpressionVisitor
    {
        protected Stack<string> _StringStack = new Stack<string>();
        protected bool IsBinaryOperator = false;
        public  string Condition()
        {
            string condition = string.Concat(this._StringStack.ToArray());
            this._StringStack.Clear();
            return condition;
        }

        /// <summary>
        /// 如果是二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null) throw new ArgumentNullException("BinaryExpression");
            this.IsBinaryOperator = false;
            this._StringStack.Push(")");
            base.Visit(node.Right);//解析右边
            this._StringStack.Push(" " + node.NodeType.ToSqlOperator() + " ");
            this.IsBinaryOperator = true;
            base.Visit(node.Left);//解析左边
            this._StringStack.Push("(");

            return node;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException("MemberExpression");
            this._StringStack.Push(" [" + node.Member.Name + "] ");
            return node;
        }
        /// <summary>
        /// 常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node == null) throw new ArgumentNullException("ConstantExpression");
            this._StringStack.Push(" '" + node.Value + "' ");
            return node;
        }
        /// <summary>
        /// 方法表达式
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) throw new ArgumentNullException("MethodCallExpression");

            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }
            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this._StringStack.Pop();
            string left = this._StringStack.Pop();
            this._StringStack.Push(String.Format(format, left, right));

            return m;
        }
    }
}
