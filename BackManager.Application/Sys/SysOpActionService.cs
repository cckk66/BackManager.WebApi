using BackManager.Common.DtoModel.Model;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysOpActionService : ISysOpActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysOpAction> _sysOpActionRepository;
        public SysOpActionService(IUnitOfWork unitOfWork, IRepository<SysOpAction> sysOpActionRepository
            )
        {
            _sysOpActionRepository = sysOpActionRepository;

            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<SysOpActionDto>> GetModelAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<SysOpActionDto>>> GridInfoAsync<Par>(Domain.DomainDrive.QueryParameter<Par> parameter) where Par : class
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<long>> InsertAsync(SysOpActionDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<long>> UpdateAsync(SysOpActionDto model)
        {
            throw new NotImplementedException();
        }
        public async Task<List<SysOpAction>> GetSysOpActionDtos()
        {
            return await _sysOpActionRepository.GetAllListAsync(m => m.DeleteFlag == (int)EDeleteFlag.正常);
        }
    }
}