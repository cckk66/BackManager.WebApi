using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model;
using BackManager.Domain;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysGroupService : ISysGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysGroup> _sysGroupRepository;
        public SysGroupService(IUnitOfWork unitOfWork, IRepository<SysGroup> sysGroupRepository)
        {
            _sysGroupRepository = sysGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<SysGroupDto>> GetModelAsync(long id)
        {
            return await Task.Run(async () =>
             {
                 SysGroup sysGroup = await _sysGroupRepository.FirstOrDefaultAsync(id);
                 return ApiResult<SysGroupDto>.Ok(AutoMapperHelper.MapTo<SysGroupDto>(sysGroup));
             });


        }

        public Task<ApiResult<PageResult<SysGroupDto>>> GridInfoAsync<Par>(BackManager.Domain.DomainDrive.QueryParameter<Par> parameter) where Par : class
        {
            Expression<Func<SysGroupParameter, bool>> lambdaWhere = m => m.DeleteFlag == 0;

            SysGroupParameter sysGroupParameter = parameter.FilterTo<SysGroupParameter>();
            lambdaWhere = lambdaWhere
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysGroupParameter.GroupName), m => m.GroupName.Contains(sysGroupParameter.GroupName))
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysGroupParameter.Remark), m => m.GroupName.Contains(sysGroupParameter.GroupName))
                                   .WhereIFOr(sysGroupParameter.UserID > 0, m => m.UserID == sysGroupParameter.UserID);
            PageResult<SysGroupDto> pageResult = _sysGroupRepository.QueryPage<SysGroupDto, SysGroupParameter>(@"
                                                                                        select 
                                                                                            sg.*,
                                                                                            sy.LoginName CreatedUserName,
                                                                                            su.LoginName   UpdatedUserName  
                                                                                        from SysGroup sg
                                                                                        left join SysUser sy on sg.CreatedUserId=sy.ID
                                                                                        left join SysUser su on sg.UpdatedUserId=su.ID",
                                                                                        lambdaWhere,
                                                                                        parameter.PageSize,
                                                                                        parameter.PageIndex,
                                                                                        parameter.OrderBy,
                                                                                        parameter.IsDesc);
            return Task.FromResult(ApiResult<PageResult<SysGroupDto>>.Ok(pageResult));

        }

        public async Task<ApiResult<long>> InsertAsync(SysGroupDto model)
        {

            SysGroup sysGroup = AutoMapperHelper.MapTo<SysGroup>(model);
            sysGroup = await _sysGroupRepository.InsertAsync(sysGroup);
            _unitOfWork.SaveChanges();
            return ApiResult<long>.Ok(sysGroup.ID);

        }

        public async Task<ApiResult<long>> UpdateAsync(SysGroupDto model)
        {
            SysGroup sysGroup = AutoMapperHelper.MapTo<SysGroup>(model);
            sysGroup = await _sysGroupRepository.UpdateAsync(sysGroup);
            _unitOfWork.SaveChanges();
            return ApiResult<long>.Ok(sysGroup.ID);
        }
    }

}