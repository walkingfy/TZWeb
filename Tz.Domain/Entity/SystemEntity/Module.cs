using System;
using System.ComponentModel.DataAnnotations;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Entity
{
    public sealed class Module : AggregateRoot
    {
        public Module()
        { }

        public Module(string name,string icon)
        {
            
        }

        #region Public Properties
        /// <summary>
        /// 模块名称
        /// </summary>
        [MaxLength(100, ErrorMessage = "超出长度限制")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 模块图标
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Icon { get; set; }
        /// <summary>
        /// 模块父ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Controller { get; set; }
        /// <summary>
        /// 功能名称（方法名）
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Action { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string LinkAddress { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出长度限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 模块类别（1：菜单；2：按钮；3：请求）
        /// </summary>
        public EnumModuleType Type { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
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
