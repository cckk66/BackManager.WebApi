using BackManager.Utility.Tool;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BackManager.Utility.Extension.ExpressionJoin
{
    public class ExpressionTool
    {
        public static object GetMemberValue(MemberInfo member, Expression expression)
        {
            var rootExpression = expression as MemberExpression;
            var memberInfos = new Stack<MemberInfo>();
            var fieldInfo = member as System.Reflection.FieldInfo;
            object reval = null;
            MemberExpression memberExpr = null;
            while (expression is MemberExpression)
            {
                memberExpr = expression as MemberExpression;
                memberInfos.Push(memberExpr.Member);
                if (memberExpr.Expression == null)
                {
                    var isProperty = memberExpr.Member.MemberType == MemberTypes.Property;
                    var isField = memberExpr.Member.MemberType == MemberTypes.Field;
                    if (isProperty)
                    {
                        try
                        {
                            reval = GetPropertyValue(memberExpr);
                        }
                        catch
                        {
                            reval = null;
                        }
                    }
                    else if (isField)
                    {
                        reval = GetFiledValue(memberExpr);
                    }
                }
                if (memberExpr.Expression == null)
                {

                }
                expression = memberExpr.Expression;
            }
            // fetch the root object reference:
            var constExpr = expression as ConstantExpression;
            if (constExpr == null)
            {
                return DynamicInvoke(rootExpression);
            }
            object objReference = constExpr.Value;
            // "ascend" back whence we came from and resolve object references along the way:
            while (memberInfos.Count > 0)  // or some other break condition
            {
                var mi = memberInfos.Pop();
                if (mi.MemberType == MemberTypes.Property)
                {
                    var objProp = objReference.GetType().GetProperty(mi.Name);
                    if (objProp == null)
                    {
                        objReference = DynamicInvoke(expression, rootExpression == null ? memberExpr : rootExpression);
                    }
                    else
                    {
                        objReference = objProp.GetValue(objReference, null);
                    }
                }
                else if (mi.MemberType == MemberTypes.Field)
                {
                    var objField = objReference.GetType().GetField(mi.Name);
                    if (objField == null)
                    {
                        objReference = DynamicInvoke(expression, rootExpression == null ? memberExpr : rootExpression);
                    }
                    else
                    {
                        objReference = objField.GetValue(objReference);
                    }
                }
            }
            reval = objReference;
            return reval;
        }

        public static object GetFiledValue(MemberExpression memberExpr)
        {
            if (!(memberExpr.Member is FieldInfo))
            {
                return DynamicInvoke(memberExpr);
            }
            object reval = null;
            FieldInfo field = (FieldInfo)memberExpr.Member;
            Check.Exception(field.IsPrivate, string.Format(" Field \"{0}\" can't be private ", field.Name));
            reval = field.GetValue(memberExpr.Member);
            if (reval != null && reval.GetType().IsClass() && reval.GetType() != UtilConstants.StringType)
            {
                var fieldName = memberExpr.Member.Name;
                var proInfo = reval.GetType().GetProperty(fieldName);
                if (proInfo != null)
                {
                    reval = proInfo.GetValue(reval, null);
                }
                var fieInfo = reval.GetType().GetField(fieldName);
                if (fieInfo != null)
                {
                    reval = fieInfo.GetValue(reval);
                }
                if (fieInfo == null && proInfo == null)
                {
                    Check.Exception(field.IsPrivate, string.Format(" Field \"{0}\" can't be private ", field.Name));
                }
            }
            return reval;
        }

        public static object GetPropertyValue(MemberExpression memberExpr)
        {
            if (!(memberExpr.Member is PropertyInfo))
            {
                return DynamicInvoke(memberExpr);
            }
            object reval = null;
            PropertyInfo pro = (PropertyInfo)memberExpr.Member;
            reval = pro.GetValue(memberExpr.Member, null);
            if (reval != null && reval.GetType().IsClass() && reval.GetType() != UtilConstants.StringType)
            {
                var fieldName = memberExpr.Member.Name;
                var proInfo = reval.GetType().GetProperty(fieldName);
                if (proInfo != null)
                {
                    reval = proInfo.GetValue(reval, null);
                }
                var fieInfo = reval.GetType().GetField(fieldName);
                if (fieInfo != null)
                {
                    reval = fieInfo.GetValue(reval);
                }
                if (fieInfo == null && proInfo == null)
                {
                    Check.Exception(true, string.Format(" Property \"{0}\" can't be private ", pro.Name));
                }
            }
            return reval;
        }

        public static object DynamicInvoke(Expression expression, MemberExpression memberExpression = null)
        {
            object value = Expression.Lambda(expression).Compile().DynamicInvoke();
            if (value != null && value.GetType().IsClass() && value.GetType() != UtilConstants.StringType && memberExpression != null)
            {
                value = Expression.Lambda(memberExpression).Compile().DynamicInvoke();
            }

            return value;
        }

    }
}
