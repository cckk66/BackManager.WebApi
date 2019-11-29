using BackManager.Utility.Extension.ExpressionJoin;
using BackManager.Utility.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BackManager.Utility.Extension.ExpressionToSql
{
    /// <summary>
    /// 条件参数化生成
    /// </summary>
    public class ConditionParBuilderVisitor : ConditionBuilderVisitor
    {
        public Expression expression { get; set; }

        /// <summary>
        /// 表达式坐标
        /// </summary>
        protected int ExpressionIndex { get; set; } = 1;
        /// <summary>
        /// 参数集合
        /// </summary>

        protected List<ParResult> ParResultList { get; set; } = new List<ParResult>();

        public SqlResult ParCondition()
        {
            SqlResult sqlResult = new SqlResult
            {
                WhereSql = base.Condition(),
                ParResultList = new List<ParResult>(this.ParResultList)
            };

            this.ExpressionIndex = 1;
            this.ParResultList.Clear();
            return sqlResult;

        }
        public override Expression Visit(Expression expression)
        {
            this.expression = expression;
            return base.Visit(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException("MemberExpression");

            Expression expression = node;



            if ((node.Expression is MemberExpression && ((MemberExpression)node.Expression).Expression.NodeType == ExpressionType.Constant)
                ||
                node.Expression.NodeType == ExpressionType.Constant
                )

            {
                string par = $"@par{ParResultList.Count()}";
                //this._StringStack.Push(" @" + node.Member.Name );
                this._StringStack.Push(par);
                ParResultList.Add(new ParResult()
                {
                    Key = par,
                    Value = ExpressionTool.GetMemberValue(node.Member, node)
                });
            }
            else
            {

                this._StringStack.Push(" [" + node.Member.Name + "] ");
            }
            return node;
        }
    }
}
