using System.Collections.Generic;

namespace BackManager.Utility.Extension.ExpressionToSql
{
    public class SqlResult
    {
        public string WhereSql { get; set; }
        public List<ParResult> ParResultList { get; set; }
    }
}
