using System;
using System.ComponentModel.DataAnnotations;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Entity
{
    public sealed class Module : AggregateRoot
    {
        public Module()
        { }

        public Module(string name)
        {
            
        }

        #region Public Properties
        [MaxLength(100, ErrorMessage = "超出长度限制")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Icon { get; set; }
        public Guid? ParentId { get; set; }
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Controller { get; set; }
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Action { get; set; }

        public string LinkAddress { get; set; }
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Remark { get; set; }
        public EnumModuleType Type { get; set; }
        public int? Sort { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public EnumIsVisible IsVisible { get; set; }
        #endregion

        #region Public Method
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
