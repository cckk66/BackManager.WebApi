using BackManager.Application;
using BackManager.Common.DtoModel;
using BackManager.Common.DtoModel.Model.Login;
using BackManager.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackManager.WebApi.Controllers
{
    public class LoginController : BaseController
    {
        #region 属性注入
        public ISysUserService _sysUserService { get; set; }
        public ISysMenuService _sysMenuService { get; set; }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param Name="loginUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginUserDto loginUserDto)
        {
            ApiResult<SysUserDto> sysUser = await _sysUserService.Login(loginUserDto);
            if (sysUser.Success)
            {
                sysUser.ApiData.Token = "安全密钥,暂未实现";
                sysUser.ApiData.MenuList = await _sysMenuService.GetUserMenuList(sysUser.ApiData.ID);
                //string menuList = Newtonsoft.Json.JsonConvert.SerializeObject(sysUser.ApiData.MenuList);
            }

            return Ok(sysUser);
        }

        /// <summary>
        /// 密保校验
        /// </summary>
        /// <param name="matrixCardDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MatrixCardValidate([FromBody]MatrixCardDto matrixCardDto)
        {
            ApiResult<bool> IsMatrixCard = await _sysUserService.MatrixCardValidate(matrixCardDto.Row, matrixCardDto.Col, matrixCardDto.CellData, matrixCardDto.UserID);
            return Ok(IsMatrixCard);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userPasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpUserPassword([FromBody]ReUserPasswordDto userPasswordDto)
        {
            return Ok(await _sysUserService.ReUserPassword(userPasswordDto));
        }
    }
}