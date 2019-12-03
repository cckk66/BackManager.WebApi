using System.Threading.Tasks;
using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Domain;

namespace BackManager.Application
{
    public interface ISysMessageService : IDataEntityAsync<SysMessageDto>
    {
        Task<ApiResult<int>> GetNewSysMessageCount();
    }
}