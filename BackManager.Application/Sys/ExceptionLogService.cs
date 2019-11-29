using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class ExceptionLogService : IExceptionLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ExceptionLog> _exceptionLogRepository;
        public ExceptionLogService(IUnitOfWork unitOfWork, IRepository<ExceptionLog> exceptionLogRepository)
        {
            _exceptionLogRepository = exceptionLogRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ExceptionLogDto>> GetModelAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<ExceptionLogDto>>> GridInfoAsync<Par>(BackManager.Domain.DomainDrive.QueryParameter<Par> parameter) where Par : class
        {
            Expression<Func<ExceptionLogParameter, bool>> lambdaWhere = null;

            ExceptionLogParameter exceptionLogParameter = parameter.FilterTo<ExceptionLogParameter>();
            DateTime SDate = string.IsNullOrEmpty(exceptionLogParameter.Dates.Split(',')[0]) ? DateTime.MinValue : DateTime.Parse(exceptionLogParameter.Dates.Split(',')[0]);
            DateTime EDate = string.IsNullOrEmpty(exceptionLogParameter.Dates.Split(',')[1]) ? DateTime.MinValue : DateTime.Parse(exceptionLogParameter.Dates.Split(',')[1]);
            lambdaWhere = lambdaWhere
                                   .WhereIFAnd(SDate!= DateTime.MinValue, m => m.CreatedAt>= SDate)
                                   .WhereIFAnd(EDate != DateTime.MinValue, m => m.CreatedAt >= EDate);
            PageResult<ExceptionLogDto> pageResult = _exceptionLogRepository.QueryPage<ExceptionLogDto, ExceptionLogParameter>(@"
                                                                                      SELECT
	                                                                                    ex.* 
                                                                                    FROM
	                                                                                    exceptionlog ex",
                                                                                        lambdaWhere,
                                                                                        parameter.PageSize,
                                                                                        parameter.PageIndex,
                                                                                        parameter.OrderBy,
                                                                                        parameter.IsDesc);
            return Task.FromResult(ApiResult<PageResult<ExceptionLogDto>>.Ok(pageResult));
        }

        public async Task<ApiResult<long>> InsertAsync(ExceptionLogDto model)
        {
            ExceptionLog exceptionLog = AutoMapperHelper.MapTo<ExceptionLog>(model);
            exceptionLog = await _exceptionLogRepository.InsertAsync(exceptionLog);
            _unitOfWork.SaveChanges();
            return ApiResult<long>.Ok(exceptionLog.ID);
        }

        public Task<ApiResult<long>> UpdateAsync(ExceptionLogDto model)
        {
            throw new NotImplementedException();
        }
    }

}