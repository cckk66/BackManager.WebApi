﻿using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitOfWork.Customer
{
    public class SysUserService : ISysUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysUser> _sysUserRepository;
        private readonly IRepository<SysUserGroup> _sysUserGroupRepository;
        private readonly IRepository<SysGroup> _sysGroupRepository;
        public SysUserService(IUnitOfWork unitOfWork, IRepository<SysUser> sysUserRepository, IRepository<SysUserGroup> sysUserGroupRepository, IRepository<SysGroup> sysGroupRepository)
        {
            _sysUserRepository = sysUserRepository;
            _sysUserGroupRepository = sysUserGroupRepository;
            _sysGroupRepository = sysGroupRepository;
            _unitOfWork = unitOfWork;
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
                return await Task.FromResult(ApiResult<SysUserDto>.Ok(new SysUserDto()
                {
                    ContractPhone = userModel.ContractPhone,
                    ID = userModel.ID,
                    LoginName = userModel.LoginName,
                    NiceName = userModel.NiceName
                }));
            }
            return await Task.FromResult(ApiResult<SysUserDto>.Error("登录错误"));
        }
        #endregion

    }
}