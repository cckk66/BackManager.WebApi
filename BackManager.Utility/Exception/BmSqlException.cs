using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BackManager.Utility
{

    public class BmSqlException : Exception
    {
        public string Sql { get; set; }
        public object Parametres { get; set; }
        public new Exception InnerException;
        public new string StackTrace;
        public new MethodBase TargetSite;
        public new string Source;

        public BmSqlException(string message)
            : base(message) { }

        public BmSqlException(string message, string sql)
            : base(message)
        {
            this.Sql = sql;
        }
        public BmSqlException(Exception ex, string message)
           : base(message)
        {
            this.InnerException = ex.InnerException;
        }
        public BmSqlException(string message, string sql, object pars)
            : base(message)
        {
            this.Sql = sql;
            this.Parametres = pars;
        }

        public BmSqlException(Exception ex, string sql, object pars)
            : base(ex.Message)
        {
            this.Sql = sql;
            this.Parametres = pars;
            this.InnerException = ex.InnerException;
            this.StackTrace = ex.StackTrace;
            this.TargetSite = ex.TargetSite;
            this.Source = ex.Source;
        }

        public BmSqlException(string message, object pars)
            : base(message)
        {
            this.Parametres = pars;
        }
    }
}
