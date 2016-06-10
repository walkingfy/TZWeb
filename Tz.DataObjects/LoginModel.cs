using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class LoginModel : DataObjectBase
    {
        [MaxLength(50,ErrorMessage="账号长度超过限制")]
        [Required(ErrorMessage = "账号不能为空")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool IsRememberMe { get; set; }
    }
}