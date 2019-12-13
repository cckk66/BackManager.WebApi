using BackManager.Application.Sys.MessageObservers.Observers;
using BackManager.Domain;
using BackManager.Domain.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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




        public Task<List<SysMenuGroupAction>> GetSysMenuGroupActionByMenuGroupIDs(List<long> MenuGroupIDs)
        {
            return Task.Run(() =>
             {
                 return _sysMenuGroupActionRepository.GetAllList().Join(MenuGroupIDs, r => r.ID, l => l, (r, l) => r).ToList();
             });
        }
    }
}
