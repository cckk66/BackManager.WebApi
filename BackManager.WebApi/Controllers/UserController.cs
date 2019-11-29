using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackManager.WebApi.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        ///退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            return Json(true);

        }
    }
}