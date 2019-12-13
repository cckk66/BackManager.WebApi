using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Domain.Model.Sys
{
    public class SysMenuGroupAction : AggregateRoot
    {
        public long MenuGroupID { get; set; }
        public long ActionID { get; set; }


    }
}
