﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Domain.Model.Sys
{
    public class SysMenuGroup: AggregateRoot
    {
        public long GroupID { get; set; }
        public long MenuID { get; set; }
    }
}
