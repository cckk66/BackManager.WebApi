using BackManager.Application.Sys.MessageObservers;
using BackManager.Application.Sys.MessageObservers.Observers;
using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using BackManager.Utility;
using BackManager.Utility.Extension.ExpressionToSql;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BackManager.Application
{
    public class SysMenuGroupService : ISysMenuGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenuGroup> _sysMenuGroupRepository;
        public SysMenuGroupService(IUnitOfWork unitOfWork, IRepository<SysMenuGroup> sysMenuGroupRepository)
        {
            _sysMenuGroupRepository = sysMenuGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<List<SysMenuGroup>> GetSysMenuGroupsByGroupID(long GroupID)
        {
            return _sysMenuGroupRepository.GetAllListAsync(m => m.GroupID == GroupID);
        }
    }
}
