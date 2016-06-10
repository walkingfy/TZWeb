using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class AccountDataObject:DataObjectBase
    {
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(50,ErrorMessage = "用户名长度超过限制")]
        public string Name { get; set; }
        [MaxLength(32,ErrorMessage = "密码长度超过限制")]
        public string Password { get; set; }
        [MaxLength(50,ErrorMessage = "邮箱长度超过限制")]
        public string Email { get; set; }
        [MaxLength(14,ErrorMessage = "联系方式长度超过限制")]
        public  string Phone { get; set; }
        [MaxLength(12,ErrorMessage = "真实姓名长度超过限制")]
        public string RealName { get; set; }
        [MaxLength(200,ErrorMessage = "备注超过长度限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 用户所属角色ID
        /// </summary>
        public List<Guid> RoleIds { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}