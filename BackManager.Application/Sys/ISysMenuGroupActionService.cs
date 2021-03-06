﻿using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysMenuGroupActionService
    {
        Task<List<SysMenuGroupActionDto>> GetSysMenuGroupActionByMenuGroupIDs(List<SysMenuGroup> MenuGroupIDs);



        Task<bool> Inserts(List<SysMenuGroupAction> newSysMenuGroupActions);
    }
}