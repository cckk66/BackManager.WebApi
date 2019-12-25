using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackManager.Common.DtoModel.Model.Login
{
    /// <summary>
    /// 登录提交类
    /// </summary>
    public class LoginUserDto
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        [Required(ErrorMessage = "登录名称不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "登录密码不能为空")]
        public string Password { get; set; }

       
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ReUserPasswordDto : LoginUserDto
    {
        [Required(ErrorMessage = "密码不能为空！")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "确认密码不能为空！")]
        public string ConfirmPassword { get; set; }

    }
    /// <summary>
    /// 密码校验类
    /// </summary>
    public class MatrixCardDto
    {
        public int[] Row { get; set; }
        public int[] Col { get; set; }
        public int[] CellData { get; set; }
        public long UserID { get; set; }
    }
}
