using System.ComponentModel.DataAnnotations;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Entity
{
    /// <summary>
    /// 角色聚合根
    /// </summary>
    public class Role:AggregateRoot
    {
        public Role()
        {
        }

        public Role(string name, string remark, EnumIsVisible isVisible)
        {
            this.Name = name;
            this.Remark = remark;
            this.IsVisible = isVisible;
        }

        #region Public Properties
        [MaxLength(50,ErrorMessage = "长度超出限制")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        [MaxLength(200,ErrorMessage = "长度超出限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public EnumIsVisible IsVisible { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 启用账户
        /// </summary>
        public void Enable()
        {
            this.IsVisible= EnumIsVisible.Can;
        }
        /// <summary>
        /// 禁用账户
        /// </summary>
        public void Disable()
        {
            this.IsVisible= EnumIsVisible.Not;
        }
        #endregion
    }
}
