using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysMenuGroupActionService
    {
        Task<List<SysMenuGroupAction>> GetSysMenuGroupActionByMenuGroupIDs(List<long> MenuGroupIDs);
 

        Task<bool> Inserts(List<SysMenuGroupAction> newSysMenuGroupActions);
    }
}