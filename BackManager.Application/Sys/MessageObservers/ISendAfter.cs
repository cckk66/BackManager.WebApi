using BackManager.Common.DtoModel.Model.SysModel;
using System.Threading.Tasks;

namespace BackManager.Application.Sys.MessageObservers
{

    /// <summary>
    /// 消息发送之后
    /// </summary>
    public interface ISendAfter
    {
       Task<SysMessageDto> SendAfter(SysMessageDto SysMessage);
    }
}
