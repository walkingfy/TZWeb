using System;
using System.ComponentModel.DataAnnotations;

using Tz.Domain.ValueObject;
using Tz.Core.Exceptions;

namespace Tz.Domain.Entity
{
    /// <summary>
    /// 帐号聚合根
    /// </summary>
    public class Account : AggregateRoot
    {
        public Account()
        {

        }

        public Account(string name, string password, string email, string phone, string realName, string remark,
            EnumIsVisible isVisible)
        {
            Name = name;
            Password = password;
            Email = email;
            Phone = phone;
            RealName = realName;
            Remark = remark;
            IsVisible = isVisible;
            CreateTime = DateTime.Now;
        }


        #region Public Properties
        /// <summary>
        /// 账号
        /// </summary>
        [MaxLength(50,ErrorMessage ="帐号长度超过限制")]
        [Required(ErrorMessage ="帐号不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage ="密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [EmailAddress(ErrorMessage ="邮箱地址错误")]
        public string Email { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [MaxLength(14,ErrorMessage ="电话号码超出长度")]
        public string Phone { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [MaxLength(20,ErrorMessage ="真实姓名超出长度")]
        public string RealName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200,ErrorMessage ="备注超出长度限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public EnumIsVisible IsVisible { get; set; }
        #endregion

        #region Public Method
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == Password)
            {
                Password = newPassword;
            }
            else
            {
                throw new CustomException("原密码与现密码不一致");
            }
        }
        /// <summary>
        /// 启用账户
        /// </summary>
        public void Enable()
        {
            this.IsVisible = EnumIsVisible.Can;
        }

        /// <summary>
        /// 禁用账户
        /// </summary>
        public void Disable()
        {
            this.IsVisible = EnumIsVisible.Not;
        }
        #endregion
    }
}
