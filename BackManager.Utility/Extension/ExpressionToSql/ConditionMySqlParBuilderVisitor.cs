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
    public class ConditionMySqlParBuilderVisitor : ConditionBuilderVisitor
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
        /// 方法表达式
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) throw new ArgumentNullException("MethodCallExpression");

            string format = "({0} LIKE {1})";;
          
            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this._StringStack.Pop();
            string left = this._StringStack.Pop();

            foreach (ParResult  parResult in ParResultList)
            {
                if (parResult.Key== right)
                {
                    switch (m.Method.Name)
                    {
                        case "StartsWith":
                            parResult.Value = $"{parResult.Value}%";
                            break;

                        case "Contains":
                            parResult.Value = $"%{parResult.Value}%"; 
                            break;

                        case "EndsWith":
                            parResult.Value = $"%{parResult.Value}"; 
                            break;

                        default:
                            throw new NotSupportedException(m.NodeType + " is not supported!");
                    }
                    break;
                }
            }
           

            this._StringStack.Push(String.Format(format, left, right));

            return m;
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

                this._StringStack.Push(node.Member.Name);
            }
            return node;
        }
    }
}
