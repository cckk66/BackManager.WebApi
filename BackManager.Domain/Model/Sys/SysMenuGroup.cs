using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Domain.Model.Sys
{
    public class SysMenuGroup: AggregateRoot
    {
        public int GroupID { get; set; }
        public int MenuID { get; set; }
    }
}
