using BackManager.Application.Sys.MessageObservers.Observers;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BackManager.Common.DtoModel.Model.SysModel;

namespace BackManager.Application
{
    public class SysMenuGroupActionService : ISysMenuGroupActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenuGroupAction> _sysMenuGroupActionRepository;
        public SysMenuGroupActionService(IUnitOfWork unitOfWork, IRepository<SysMenuGroupAction> sysMenuGroupActionRepository)
        {
            _sysMenuGroupActionRepository = sysMenuGroupActionRepository;
            _unitOfWork = unitOfWork;
        }




        public Task<List<SysMenuGroupActionDto>> GetSysMenuGroupActionByMenuGroupIDs(List<SysMenuGroup> MenuGroupIDs)
        {
            return Task.Run(() =>
             {

                 return _sysMenuGroupActionRepository.GetAllList()
                 .Join<SysMenuGroupAction, SysMenuGroup, long, SysMenuGroupActionDto>(MenuGroupIDs, r => r.MenuGroupID, l => l.ID, (r, l) => new SysMenuGroupActionDto
                 {
                     ActionID = r.ActionID,
                     MenuID = l.MenuID,
                     MenuGroupActionID = r.ID
                 }).ToList();

             });
        }

        public async Task<bool> Inserts(List<SysMenuGroupAction> newSysMenuGroupActions)
        {
            return await _sysMenuGroupActionRepository.BulkInsert(newSysMenuGroupActions) > 0;

        }
    }
}
