using System;
using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class ArticleDataObject:DataObjectBase
    {
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
        /// <summary>
        /// 文章类别
        /// </summary>
        public Guid ArticleCategoryId { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度超出限制")]
        public string Description { get; set; }
        /// <summary>
        /// 创建者名称
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}