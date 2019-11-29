using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysMessageService : ISysMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMessage> _sysMessageRepository;
        public SysMessageService(IUnitOfWork unitOfWork, IRepository<SysMessage> sysMessageRepository)
        {
            _sysMessageRepository = sysMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResult<SysMessageDto>> GetModelAsync(long id)
        {
            return await Task.Run(async () =>
            {
                SysMessage sysMessage = await _sysMessageRepository.FirstOrDefaultAsync(id);
                return ApiResult<SysMessageDto>.Ok(AutoMapperHelper.MapTo<SysMessageDto>(sysMessage));
            });

        }

        public Task<ApiResult<PageResult<SysMessageDto>>> GridInfoAsync<Par>(Domain.DomainDrive.QueryParameter<Par> parameter) where Par : class
        {
            Expression<Func<SysMessagePar, bool>> lambdaWhere = m => m.DeleteFlag == 0;

            SysMessagePar sysMessagePar = parameter.FilterTo<SysMessagePar>();
            DateTime dateTime = DateTime.Now;
            lambdaWhere = lambdaWhere
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysMessagePar.Title), m => m.Title.Contains(sysMessagePar.Title))
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysMessagePar.Content), m => m.Content.Contains(sysMessagePar.Content))
                                    .WhereIFAnd
                                    (
                                        sysMessagePar.PutStartDate != null && sysMessagePar.PutStartDate != DateTime.MinValue,
                                        m => m.PutStartDate >= dateTime
                                    )
                                    .WhereIFAnd(
                                        sysMessagePar.PutEndDate != null && sysMessagePar.PutEndDate != DateTime.MinValue,
                                        m => m.PutEndDate <= dateTime
                                    );


            //var sysUserQueryable = _sysUserRepository.GetAll().Where(m => m.LoginName.Contains("te")).ToList();



            PageResult<SysMessageDto> pageResult = _sysMessageRepository.QueryPage<SysMessageDto, SysMessagePar>(@"
                                                                                       SELECT
	                                                                                    sm.ID,
	                                                                                    sm.Title,
	                                                                                    sm.Content,
	                                                                                    sm.PutStartDate,
	                                                                                    sm.PutEndDate,          
                                                                                        sm.DeleteFlag,
                                                                                        sy.LoginName CreatedUserName,
                                                                                        su.LoginName   UpdatedUserName 
                                                                                    FROM
	                                                                                    sysmessage sm
                                                                                        left join SysUser sy on sm.CreatedUserId=sy.ID
                                                                                        left join SysUser su on sm.UpdatedUserId=su.ID",
                                                                                        lambdaWhere,
                                                                                        parameter.PageSize,
                                                                                        parameter.PageIndex,
                                                                                        parameter.OrderBy,
                                                                                        parameter.IsDesc);
            return Task.FromResult(ApiResult<PageResult<SysMessageDto>>.Ok(pageResult));
        }

        public async Task<ApiResult<long>> InsertAsync(SysMessageDto model)
        {
            SysMessage sysMessage = AutoMapperHelper.MapTo<SysMessage>(model);
            sysMessage = await _sysMessageRepository.InsertAsync(sysMessage);
            _unitOfWork.SaveChanges();
            return await Task.FromResult(ApiResult<long>.Ok(sysMessage.ID));
        }

        public async Task<ApiResult<long>> UpdateAsync(SysMessageDto model)
        {
            SysMessage sysMessage = AutoMapperHelper.MapTo<SysMessage>(model);
            sysMessage = await _sysMessageRepository.UpdateAsync(sysMessage, m => new { m.Title, m.Content, m.PutStartDate, m.PutEndDate });
            _unitOfWork.SaveChanges();
            return await Task.FromResult(ApiResult<long>.Ok(sysMessage.ID));
        }
    }
}
