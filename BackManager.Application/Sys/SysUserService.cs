using BackManager.Application.Sys;
using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using BackManager.Utility.MatrixCard;
using BackManager.Utility.MySecretInsuranceCard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysUserService : ISysUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecretInsuranceCard _secretInsuranceCard;
        private readonly IRepository<SysUser> _sysUserRepository;
        private readonly IRepository<SysUserGroup> _sysUserGroupRepository;
        private readonly IRepository<SysGroup> _sysGroupRepository;
        private readonly ISysUserSecretInsuranceCardService _sysUserSecretInsuranceCardService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SysUserService(
            IUnitOfWork unitOfWork
            , IRepository<SysUser> sysUserRepository
            , IRepository<SysUserGroup> sysUserGroupRepository
            , IRepository<SysGroup> sysGroupRepository
            , ISecretInsuranceCard secretInsuranceCard
            , ISysUserSecretInsuranceCardService sysUserSecretInsuranceCardService
            , IHttpContextAccessor httpContextAccessor
            )
        {
            _sysUserRepository = sysUserRepository;
            _sysUserGroupRepository = sysUserGroupRepository;
            _sysGroupRepository = sysGroupRepository;
            _unitOfWork = unitOfWork;
            _secretInsuranceCard = secretInsuranceCard;
            _sysUserSecretInsuranceCardService = sysUserSecretInsuranceCardService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ApiResult<long>> DeleteAsync(long[] ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResult<SysUserDto>> GetModelAsync(long id)
        {

            IQueryable<SysUser> sysUserQueryable = _sysUserRepository.GetAll();
            IQueryable<SysUserGroup> sysUserGroupQueryable = _sysUserGroupRepository.GetAll();
            IQueryable<SysGroup> sysGroupQueryable = _sysGroupRepository.GetAll();

            var userModel = from u in sysUserQueryable
                            join uug in sysUserGroupQueryable on u.ID equals uug.UserID
                            into uuug
                            from uuugModel in uuug.DefaultIfEmpty()
                            join ug in sysGroupQueryable on uuugModel.GroupID equals ug.ID
                            into uug
                            from uugModel in uug.DefaultIfEmpty()
                            where u.ID == id
                            select new SysUserDto()
                            {
                                ID = u.ID,
                                LoginName = u.LoginName,
                                DepartmentID = u.DepartmentID,
                                NiceName = u.NiceName,
                                ContractPhone = u.ContractPhone,
                                GroupName = uugModel.GroupName
                            };
            SysUserDto sysUserDto = userModel.FirstOrDefault();
            if (sysUserDto != null)
            {
                return Task.FromResult(ApiResult<SysUserDto>.Ok(sysUserDto));
            }

            return Task.FromResult(ApiResult<SysUserDto>.Error());


        }

        public Task<ApiResult<PageResult<SysUserDto>>> GridInfoAsync<Par>(BackManager.Domain.DomainDrive.QueryParameter<Par> parameter) where Par : class
        {
            Expression<Func<SysUserPar, bool>> lambdaWhere = m => m.DeleteFlag == 0;

            SysUserPar sysUserPar = parameter.FilterTo<SysUserPar>();
            lambdaWhere = lambdaWhere
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysUserPar.LoginName), m => m.LoginName.Contains(sysUserPar.LoginName))
                                   .WhereIFAnd(!string.IsNullOrEmpty(sysUserPar.ContractPhone), m => m.ContractPhone.Contains(sysUserPar.ContractPhone));

            //var sysUserQueryable = _sysUserRepository.GetAll().Where(m => m.LoginName.Contains("te")).ToList();



            PageResult<SysUserDto> pageResult = _sysUserRepository.QueryPage<SysUserDto, SysUserPar>(@"SELECT
                                                                                        sy.ID,
                                                                                        sy.UserType,
                                                                                        sy.NiceName,
                                                                                        sy.LoginName,
                                                                                        sy.ContractPhone,
                                                                                        sg.GroupName,
                                                                                        sy.DeleteFlag
                                                                                    FROM
                                                                                        sysuser sy
                                                                                        LEFT JOIN sysusergroup ssg ON sy.id = ssg.UserID
                                                                                        LEFT JOIN sysgroup sg ON ssg.GroupID = sg.id",
                                                                                        lambdaWhere,
                                                                                        parameter.PageSize,
                                                                                        parameter.PageIndex,
                                                                                        parameter.OrderBy,
                                                                                        parameter.IsDesc);
            return Task.FromResult(ApiResult<PageResult<SysUserDto>>.Ok(pageResult));


        }

        public async Task<ApiResult<long>> InsertAsync(SysUserDto model)
        {
            SysUser sysUser = AutoMapperHelper.MapTo<SysUser>(model);
            sysUser = await _sysUserRepository.InsertAsync(sysUser);
            _unitOfWork.SaveChanges();
            return await Task.FromResult(ApiResult<long>.Ok(sysUser.ID));
        }



        public async Task<ApiResult<long>> UpdateAsync(SysUserDto model)
        {
            SysUser sysUser = AutoMapperHelper.MapTo<SysUser>(model);
            sysUser = await _sysUserRepository.UpdateAsync(sysUser, m => new { m.LoginName, m.NiceName, m.ContractPhone });
            _unitOfWork.SaveChanges();
            return await Task.FromResult(ApiResult<long>.Ok(sysUser.ID));
        }

        public SysUser User()
        {
            return _sysUserRepository.FirstOrDefault(11);
        }

        #region 用户登录
        public async Task<ApiResult<SysUserDto>> Login(LoginUserDto loginUserDto)
        {

            var userModel =
                _sysUserRepository.GetAll().Where(m => m.LoginName == loginUserDto.UserName
                && m.Password == loginUserDto.Password.ToMD5Encoding()).FirstOrDefault();


            if (userModel != null)
            {
                (int[] Row, int[] Col, string PromptingLanguage) RandomCells = await _sysUserSecretInsuranceCardService.PickRandomCells(userModel.ID);

                return await Task.FromResult(ApiResult<SysUserDto>.Ok(new SysUserDto()
                {
                    ContractPhone = userModel.ContractPhone,
                    ID = userModel.ID,
                    LoginName = userModel.LoginName,
                    NiceName = userModel.NiceName,
                    Row = RandomCells.Row,
                    Col = RandomCells.Col,
                    PromptingLanguage = RandomCells.PromptingLanguage,
                })); ;
            }
            return await Task.FromResult(ApiResult<SysUserDto>.Error("登录错误"));
        }


        #endregion
        /// <summary>
        /// 获取密保
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ApiResult<SysUserSecretInsuranceCard>> GetMatrixCard(long userID)
        {
            SysUserSecretInsuranceCard sysUserSecretInsuranceCard = await _sysUserSecretInsuranceCardService.LoadSysUserSecretInsuranceCardByUserID(userID);
            if (sysUserSecretInsuranceCard == null)
            {
                (int Rows, int Cols, (string Head, string Body) CellData) MatrixCard = _secretInsuranceCard.Create();
                sysUserSecretInsuranceCard = new SysUserSecretInsuranceCard
                {
                    Rows = MatrixCard.Rows,
                    Cols = MatrixCard.Cols,
                    SecretInsuranceHead = MatrixCard.CellData.Head,
                    SecretInsuranceBody = MatrixCard.CellData.Body,
                    UserID = userID,
                    CreatedAt = DateTime.Now,
                    CreatedUserId = userID
                };
                ApiResult<long> apiResult = await _sysUserSecretInsuranceCardService.InsertAsync(sysUserSecretInsuranceCard);
                if (apiResult.Success)
                {
                    return await Task.FromResult(ApiResult<SysUserSecretInsuranceCard>.Ok(sysUserSecretInsuranceCard));
                }
            }
            else {
                return await Task.FromResult(ApiResult<SysUserSecretInsuranceCard>.Ok(sysUserSecretInsuranceCard));
            }
            return await Task.FromResult(ApiResult<SysUserSecretInsuranceCard>.Error("获取令牌错误"));
        }
        /// <summary>
        /// 密保校验
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="CellData"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> MatrixCardValidate(int[] Row, int[] Col, int[] CellData, long UserID)
        {

            bool IsMatrixCardValidate = await _sysUserSecretInsuranceCardService.MatrixCardValidate(Row, Col, CellData, UserID);
            if (!IsMatrixCardValidate)
            {
                return await Task.FromResult(ApiResult<bool>.Error("密保验证错误！"));
            }
            return await Task.FromResult(ApiResult<bool>.Ok(true));
        }

        public async Task<ApiResult<string>> ReUserPassword(ReUserPasswordDto userPasswordDto)
        {
            _unitOfWork.BginTran();
            SysUser dbSysUser = await _sysUserRepository.FirstOrDefaultAsync(m => m.LoginName.Equals(userPasswordDto.UserName) && m.Password.Equals(userPasswordDto.OldPassword.ToMD5Encoding()));

            if (dbSysUser != null)
            {
                if (userPasswordDto.ConfirmPassword.Equals(userPasswordDto.Password))
                {

                    dbSysUser.Password = userPasswordDto.Password.ToMD5Encoding();
                    dbSysUser.UpdatedAt = DateTime.Now;
                    dbSysUser.UpdatedUserId = 1;
                    var ret = await _sysUserRepository.UpdateAsync(dbSysUser, m => m.ID == dbSysUser.ID);
                    if (ret.ID > 0)
                    {

                        return await Task.FromResult(ApiResult<string>.Ok(""));
                    }
                    else
                    {
                        return await Task.FromResult(ApiResult<string>.Error("修改密码失败,稍后再试！"));
                    }
                }
                return await Task.FromResult(ApiResult<string>.Error("两次密码输入不一致！"));
            }
            else
            {
                return await Task.FromResult(ApiResult<string>.Error("用户不存在或密码不正确！"));
            }
        }
    }
}