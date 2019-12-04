using BackManager.Common.DtoModel.Model.SysModel;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using BackManager.Application.Signal;
using Microsoft.Extensions.Caching.Redis;

namespace BackManager.Application.Sys.MessageObservers.Observers
{
    public class SysHubObservers : ISendAfter
    {
        private readonly IHubContext<SysHub> _sysHub;
        private readonly IServiceStackRedisCache _serviceStackRedisCache;
        public SysHubObservers(IHubContext<SysHub> sysHub, IServiceStackRedisCache serviceStackRedisCache)
        {
            _sysHub = sysHub;
            _serviceStackRedisCache = serviceStackRedisCache;
        }
       

        public async Task<SysMessageDto> SendAfter(SysMessageDto SysMessage)
        {
            await _sysHub.Clients.SysMessage(SysMessage);
            return SysMessage;
        }
    }
}
