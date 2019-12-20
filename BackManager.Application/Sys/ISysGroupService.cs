using BackManager.Common.DtoModel;
using BackManager.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysGroupService : IDataEntityAsync<SysGroupDto>
    {
        Task<ApiResult<(List<GroupMenuDto> TreeData, List<string> ExpandedKeys, List<string> CheckdKeys)>> GetGroupMenuDto(long GroupID);
        Task<ApiResult<bool>> SaveGroupMenuPower(long GroupID, List<GroupMenuDto> groupMenuDtos);
    }
}