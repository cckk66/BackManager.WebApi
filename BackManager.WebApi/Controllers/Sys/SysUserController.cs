using BackManager.Application;
using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.SysModel.QueryParameter;
using BackManager.Domain.DomainDrive;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackManager.WebApi.Controllers.Sys
{
    public class SysUserController : BaseController
    {
        private readonly ISysUserService _sysUserService;
        public SysUserController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 用户测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSysUser(long ID)
        {
            return Ok(await _sysUserService.GetModelAsync(ID));
        }
        /// <summary>
        /// 用户新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SysUserAdd([FromBody]SysUserDto model)
        {
            return Ok(await _sysUserService.InsertAsync(model));
        }
        /// <summary>
        /// 用户修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SysUserUpdate([FromBody]SysUserDto model)
        {
            return Ok(await _sysUserService.UpdateAsync(model));
        }
        /// <summary>
        /// 用户分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SysUserPage([FromBody]QueryParameter<SysUserPar> queryParameter)
        {
            return Ok(await _sysUserService.GridInfoAsync(queryParameter));
        }

        /// <summary>
        /// 用户测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogOut()
        {
            return Ok(new { });
        }
        /// <summary>
        /// 获取密保卡
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMatrixCard(long UserID)
        {
            return Ok(await _sysUserService.GetMatrixCard(UserID));
        }
    }
}