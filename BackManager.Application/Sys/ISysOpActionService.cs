using BackManager.Common.DtoModel.Model;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public interface ISysOpActionService : IDataEntityAsync<SysOpActionDto>
    {
        Task<List<SysOpAction>> GetSysOpActionDtos();
    }
}