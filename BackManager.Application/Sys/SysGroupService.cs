using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model;
using BackManager.Common.DtoModel.Model.SysModel;
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

        public SysGroupService(
            IUnitOfWork unitOfWork
            , IRepository<SysGroup> sysGroupRepository, ISysMenuService sysMenuService,
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
        public async Task<ApiResult<(List<GroupMenuDto> TreeData, List<long> ExpandedKeys, List<long> CheckdKeys)>> GetGroupMenuDto(long GroupID)
        {

            List<SysMenuDto> sysMenus = await _sysMenuService.GetSysMenus();
            List<GroupMenuDto> groupMenuDtos = AutoMapperHelper.MapToList<SysMenuDto, GroupMenuDto>(sysMenus).ToList();
            List<SysMenuGroup> sysMenuGroups = await _sysMenuGroupService.GetSysMenuGroupsByGroupID(GroupID);
            List<SysMenuGroupActionDto> sysMenuGroupActions = await _sysMenuGroupActionService.GetSysMenuGroupActionByMenuGroupIDs(sysMenuGroups);
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
            newGroupMenuDtos = SetSysMenuDto(0, groupMenuDtos, gmOpActions, sysMenuGroupActions);
            return ApiResult<(List<GroupMenuDto> TreeData, List<long> ExpandedKeys, List<long> CheckdKeys)>.Ok((
                newGroupMenuDtos,
                sysMenus.Select(m => m.ID).ToList(),
                sysMenuGroupActions.Select(m => m.MenuGroupActionID).ToList()));

        }
        private List<GroupMenuDto> SetSysMenuDto(long FatherID, List<GroupMenuDto> dbSysMenuDtos, List<GroupMenuDto> gmOpActions, List<SysMenuGroupActionDto> sysMenuGroupActions)
        {

            if (dbSysMenuDtos.Any(m => m.FatherID == FatherID))
            {
                List<GroupMenuDto> Children = null;


                Children = dbSysMenuDtos.Where(m => m.FatherID == FatherID).ToList();
                Children.ForEach(m =>
                {
                    m.Children = SetSysMenuDto(m.ID, dbSysMenuDtos, gmOpActions, sysMenuGroupActions);
                });

                Children = Children.OrderBy(m => m.Orderby).ToList();
                Children.ForEach(m => m.NodeKey = m.ID);
                return Children;
            }

            List<GroupMenuDto> newGmOpActions = AutoMapperHelper
                .MapToList<GroupMenuDto, GroupMenuDto>(gmOpActions)
                .ToList();

            newGmOpActions.ForEach(m =>
            {
                m.FatherID = FatherID;
                m.NodeKey = sysMenuGroupActions.FirstOrDefault(fo => fo.MenuID == FatherID && fo.ActionID == m.ID)?.MenuGroupActionID;

            });
            return newGmOpActions;
        }

        public async Task<ApiResult<bool>> SaveGroupMenuPower(long GroupID, List<GroupMenuDto> groupMenuDtos)
        {
            try
            {
                _unitOfWork.BginTran();
                bool clearMenuGroup = await _sysMenuGroupService.ClearByGroupID(GroupID);

                _unitOfWork.SaveChanges();
                if (groupMenuDtos.Count > 0)
                {


                    List<SysMenuGroup> sysMenuGroups = new List<SysMenuGroup>();
                    Dictionary<long, List<SysMenuGroupAction>> sysMenuGroupActions = new Dictionary<long, List<SysMenuGroupAction>>();
                    if (groupMenuDtos != null)
                    {
                        groupMenuDtos.ForEach(m =>
                        {
                            if (m.FatherID > 0)
                            {
                                if (!sysMenuGroups.Any(g => g.MenuID == m.FatherID))
                                {
                                    sysMenuGroups.Add(new SysMenuGroup
                                    {
                                        GroupID = GroupID,
                                        MenuID = Convert.ToInt64(m.FatherID)
                                    });
                                }

                                switch (m.GroupMenuType)
                                {
                                    case EGroupMenuType.Menu:
                                        sysMenuGroups.Add(new SysMenuGroup
                                        {
                                            GroupID = GroupID,
                                            MenuID = m.ID
                                        });
                                        break;
                                    case EGroupMenuType.MenuButton:
                                        long FatherID = Convert.ToInt64(m.FatherID);
                                        if (sysMenuGroupActions.ContainsKey(FatherID))
                                        {
                                            sysMenuGroupActions[FatherID].Add(new SysMenuGroupAction
                                            {
                                                ActionID = m.ID
                                            });
                                        }
                                        else
                                        {
                                            sysMenuGroupActions[FatherID] = new List<SysMenuGroupAction>()
                                            {
                                                new SysMenuGroupAction{ActionID = m.ID }
                                            };
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (m.GroupMenuType == EGroupMenuType.Menu)
                                {
                                    sysMenuGroups.Add(new SysMenuGroup
                                    {
                                        GroupID = GroupID,
                                        MenuID = m.ID
                                    });
                                }
                            }

                        });
                        List<SysMenuGroup> newSysMenuGroups = await _sysMenuGroupService.Inserts(sysMenuGroups);
                        _unitOfWork.SaveChanges();
                        List<SysMenuGroupAction> newSysMenuGroupActions = new List<SysMenuGroupAction>();
                        newSysMenuGroups.ForEach(m =>
                        {
                            if (m.ID > 0)
                            {
                                if (sysMenuGroupActions.ContainsKey(m.MenuID))
                                {
                                    sysMenuGroupActions[m.MenuID].ForEach(t =>
                                    {
                                        t.MenuGroupID = m.ID;
                                    });
                                    newSysMenuGroupActions.AddRange(sysMenuGroupActions[m.MenuID]);
                                }
                            }
                        });

                        bool isSysMenuGroupActions = await _sysMenuGroupActionService.Inserts(newSysMenuGroupActions);
                    }
                }
                _unitOfWork.Commit();
                return ApiResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }

}