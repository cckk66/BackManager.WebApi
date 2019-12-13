using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysGroupService : ISysGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysGroup> _sysGroupRepository;

        private readonly ISysMenuService _sysMenuService;
        private readonly ISysMenuGroupService _sysMenuGroupService;
        private readonly ISysMenuGroupActionService _sysMenuGroupActionService;
        private readonly ISysOpActionService _sysOpActionService;
        
        public SysGroupService(IUnitOfWork unitOfWork, IRepository<SysGroup> sysGroupRepository, ISysMenuService sysMenuService,
            ISysMenuGroupService sysMenuGroupService,
            ISysMenuGroupActionService sysMenuGroupActionService,
            ISysOpActionService sysOpActionService
            )
        {
            _sysGroupRepository = sysGroupRepository;
            _unitOfWork = unitOfWork;
            _sysMenuService = sysMenuService;
            _sysMenuGroupService = sysMenuGroupService;
            _sysMenuGroupActionService = sysMenuGroupActionService;
            _sysOpActionService = sysOpActionService;
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
        public async Task<ApiResult<List<GroupMenuDto>>> GetGroupMenuDto(long GroupID)
        {
            List<SysMenuDto> sysMenus = await _sysMenuService.GetSysMenus();
            List<GroupMenuDto> groupMenuDtos = AutoMapperHelper.MapToList<SysMenuDto, GroupMenuDto>(sysMenus).ToList();
            List<SysMenuGroup> sysMenuGroups = await _sysMenuGroupService.GetSysMenuGroupsByGroupID(GroupID);
            List<long> MenuGroupIDs = sysMenuGroups.Select(m => m.ID).ToList();
            List<SysMenuGroupAction> sysMenuGroupActions = await _sysMenuGroupActionService.GetSysMenuGroupActionByMenuGroupIDs(MenuGroupIDs);
            List<GroupMenuDto> newGroupMenuDtos = new List<GroupMenuDto>();
            List<GroupMenuDto> gmOpActions = new List<GroupMenuDto>();
            {
                List<SysOpAction> sysOpActions = await _sysOpActionService.GetSysOpActionDtos();
                sysOpActions.ForEach(soa =>
                {
                    gmOpActions.Add(new GroupMenuDto
                    {
                        ID = soa.ID,
                        Name = soa.ActionName,
                        Icon = soa.ActionIcon,
                        GroupMenuType = EGroupMenuType.MenuButton
                    });
                });
            }
            newGroupMenuDtos= SetSysMenuDto(0, groupMenuDtos, gmOpActions);
            return ApiResult<List<GroupMenuDto>>.Ok(newGroupMenuDtos);

        }
        private List<GroupMenuDto> SetSysMenuDto(long FatherID, List<GroupMenuDto> dbSysMenuDtos, List<GroupMenuDto> gmOpActions)
        {

            if (dbSysMenuDtos.Any(m => m.FatherID == FatherID))
            {
                List<GroupMenuDto> Children = null;

                if (FatherID == 0)
                {
                    Children = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
                    Children.ForEach(m =>
                    {
                        m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, gmOpActions);
                    });
                }
                else
                {
                    Children = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
                    Children.ForEach(m =>
                    {
                        m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, gmOpActions);
                    });
                }
                return Children.OrderBy(m => m.Orderby).ToList();
            }

            return gmOpActions;
        }

        //private List<GroupMenuDto> SetSysMenuDto(long FatherID, List<GroupMenuDto> dbSysMenuDtos, List<GroupMenuDto> groupMenuDtos, List<GroupMenuDto> gmOpActions)
        //{

        //    if (dbSysMenuDtos.Any(m => m.FatherID == FatherID))
        //    {
        //        List<GroupMenuDto> Children = null;

        //        Children = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
        //        Children.ForEach(m =>
        //        {

        //            m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, groupMenuDtos, gmOpActions);
        //            if (m.Children.Count==0)
        //            {
        //                m.Children = gmOpActions;
        //            }
        //        });
        //        Children = Children.OrderBy(m => m.Orderby).ToList();

        //        return Children;
        //    }
        //    else
        //    {

        //    }


        //    return groupMenuDtos;
        //}
    }

}