using BackManager.Application;
using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model;
using BackManager.Domain.DomainDrive;
using BackManager.Utility.Filter;
using BackManager.Utility.Filter.FormRepeatSubmitIntercept;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackManager.WebApi.Controllers.Sys
{
    public class SysGroupController : BaseController
    {
        private readonly ISysGroupService _sysGroupService;
        public SysGroupController(ISysGroupService sysGroupService)
        {
            _sysGroupService = sysGroupService;
        }

        /// <summary>
        /// 用户分组分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> SysGroupPage([FromBody]QueryParameter<SysGroupParameter> queryParameter)
        {
            return Ok(await _sysGroupService.GridInfoAsync(queryParameter));
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="SysGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(FormRepeatSubmitInterceptFilter))]
        public async Task<IActionResult> SysGroupUpdate([FromBody]SysGroupDto SysGroup)
        {
            //SysGroup.UpdatedUserId = base.loginUser.UserID;
            //SysGroup.UpdatedAt = DateTime.Now;
            return Ok(await _sysGroupService.UpdateAsync(SysGroup));
        }

        /// <summary>
        /// 新增/修改分组
        /// </summary>
        /// <param name="SysGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(FormRepeatSubmitInterceptFilter))]
        [TypeFilter(typeof(DataValidationActionFilter))]
        public async Task<IActionResult> SysGroupAdd([FromBody]SysGroupDto SysGroup)
        {
            if (!ModelState.IsValid)
            {
            }
            return Ok(await _sysGroupService.InsertAsync(SysGroup));
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SysGroupDelete([FromBody]long[] Ids)
        {
            return Ok(await _sysGroupService.DeleteAsync(Ids));
        }
        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSysGroup(long Id)
        {
            return Ok(await _sysGroupService.GetModelAsync(Id));
        }

    }
}