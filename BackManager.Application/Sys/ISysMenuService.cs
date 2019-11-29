using System.Collections.Generic;
using System.Threading.Tasks;
using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Common.DtoModel.Model.RouterDto;
using BackManager.Domain;

namespace BackManager.Application
{
    public interface ISysMenuService : IDataEntityAsync<SysMenuDto>
    {
        Task<RouterDto> GetUserMenuList(long iD);
        Task<ApiResult<long>> saveMenuSort(Dictionary<int, int> dicSort);
    }
}