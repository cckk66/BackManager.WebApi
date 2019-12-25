using BackManager.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackManager.WebApi.Controllers
{
    public class UserController : Controller
    {
        #region 属性注入
        public ISysUserService _sysUserService { get; set; }
        #endregion
        /// <summary>
        ///退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            return Json(true);
        }
       
        [HttpPost]
        public async Task<IActionResult> GetMatrixCard(long UserID)
        {
            return Ok(await _sysUserService.GetMatrixCard(UserID));
        }



    }
}