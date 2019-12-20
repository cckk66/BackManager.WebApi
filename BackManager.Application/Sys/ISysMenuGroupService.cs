using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysMenuGroupService
    {
        Task<List<SysMenuGroup>> GetSysMenuGroupsByGroupID(long GroupID);
        Task<bool> ClearByGroupID(long groupID);
        Task<List<SysMenuGroup>> Inserts(List<SysMenuGroup> SysMenuGroups);
    }
}