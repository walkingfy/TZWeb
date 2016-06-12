using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Tz.Domain.ValueObject;

namespace Tz.Domain.Entity
{
    /// <summary>
    /// 文章聚合根
    /// </summary>
    public class Article : AggregateRoot
    {

        public Article()
        { }


        #region Public Properties
        [MaxLength(50,ErrorMessage = "长度超出限制")]
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        [MaxLength(200,ErrorMessage = "长度超出限制")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public EnumIsVisible IsVisible { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// 显示文章
        /// </summary>
        public void Enable()
        {
            this.IsVisible = EnumIsVisible.Can;
        }
        /// <summary>
        /// 隐藏文章
        /// </summary>
        public void Disable()
        {
            this.IsVisible = EnumIsVisible.Not;
        }
        #endregion
    }
}