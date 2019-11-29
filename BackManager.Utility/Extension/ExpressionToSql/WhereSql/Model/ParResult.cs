using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BackManager.Utility.Extension.ExpressionToSql
{
   public class ParResult
    {
        public MemberExpression memberExpression { get; set; }

        public string Key { get; set; }
        public dynamic Value { get; set; }
    }
}
