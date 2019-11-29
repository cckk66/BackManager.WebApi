using BackManager.Common.DtoModel.Model;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain.DomainDrive;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitOfWork.Customer;

namespace BackManager.WebApi.Controllers.Sys
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class SysMenuController : BaseController
    {
        private readonly ISysMenuService _sysMenuService;
        public SysMenuController(ISysMenuService sysMenuService)
        {
            _sysMenuService = sysMenuService;
        }

        /// <summary>
        /// 菜单分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> SysMenuPage([FromBody]QueryParameter<SysMenuParameter> queryParameter)
        {
            return Ok(await _sysMenuService.GridInfoAsync(queryParameter));
        }

        /// <summary>
        /// 保存菜单排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> saveMenuSort([FromBody]Dictionary<int,int> dicSort)
        {
            return Ok(await _sysMenuService.saveMenuSort(dicSort));
        }

    }
}