using BackManager.Common.DtoModel.Model.SysModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackManager.Application.Sys.MessageObservers
{
    /// <summary>
    /// 发送观察
    /// </summary>
    public class SendObservers
    {
        public IList<ISendAfter> sendObservers = new List<ISendAfter>();
        public void AddObserver(ISendAfter sendAfter)
        {
            sendObservers.Add(sendAfter);
        }
        public void SendAfter(SysMessageDto SysMessage)
        {
            foreach (ISendAfter sendAfter in sendObservers)
            {
                sendAfter.SendAfter(SysMessage);
            }
        }
    }
}
