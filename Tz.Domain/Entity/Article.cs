using System;
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
        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(50,ErrorMessage = "长度超出限制")]
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public string MetaKeyWords { get; set; }
        /// <summary>
        /// SEO概述
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// 阅读次数
        /// </summary>
        public int Counter { get; set; }

        public Guid ArticleCategoryId { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200,ErrorMessage = "长度超出限制")]
        public string Description { get; set; }
        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateName { get; set; }
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