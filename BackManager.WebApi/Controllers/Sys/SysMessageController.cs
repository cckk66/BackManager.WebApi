using BackManager.Application;
using BackManager.Common.DtoModel.Model.SysModel;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain.DomainDrive;
using BackManager.Utility.Filter.FormRepeatSubmitIntercept;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackManager.WebApi.Controllers.Sys
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SysMessageController : BaseController
    {
        private readonly ISysMessageService _sysMessageService;
        public SysMessageController(ISysMessageService sysMessageService)
        {
            _sysMessageService = sysMessageService;
        }
        /// <summary>
        /// 用户系统消息分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> GetNewSysMessageCount()
        {
            return Ok(await _sysMessageService.GetNewSysMessageCount());
        }

        /// <summary>
        /// 用户系统消息分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> SysMessagePage([FromBody]QueryParameter<SysMessagePar> queryParameter)
        {
            return Ok(await _sysMessageService.GridInfoAsync(queryParameter));
        }

        /// <summary>
        /// 修改系统消息
        /// </summary>
        /// <param name="SysMessage"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(FormRepeatSubmitInterceptFilter))]
        public async Task<IActionResult> SysMessageUpdate([FromBody]SysMessageDto SysMessage)
        {


            return Ok(await _sysMessageService.UpdateAsync(SysMessage));
        }

        /// <summary>
        /// 新增/修改系统消息
        /// </summary>
        /// <param name="SysMessage"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(FormRepeatSubmitInterceptFilter))]
        public async Task<IActionResult> SysMessageAdd([FromBody]SysMessageDto SysMessage)
        {
            if (!ModelState.IsValid)
            {
            }
            return Ok(await _sysMessageService.InsertAsync(SysMessage));
        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SysMessageDelete([FromBody]long[] Ids)
        {
            return Ok(await _sysMessageService.DeleteAsync(Ids));
        }
        /// <summary>
        /// 获取系统消息信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSysMessage(long Id)
        {
            return Ok(await _sysMessageService.GetModelAsync(Id));
        }
    }
}