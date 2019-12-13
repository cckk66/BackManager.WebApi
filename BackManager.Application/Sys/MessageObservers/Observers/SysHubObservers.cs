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
            //await _sysHub.Clients.SysMessage(SysMessage);
            int Count = System.Convert.ToInt32(SysMessage.Content);
            for (int i = 0; i < Count; i++)
            {
                await _sysHub.Clients.Client(SysMessage.Title).SendAsync("testOpen", Count, (Count - (i+1)));
                //Thread.Sleep(500);
                await Task.Delay(500);
            }
            return SysMessage;
        }
    }
}
