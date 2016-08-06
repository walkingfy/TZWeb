using System;
using System.ComponentModel.DataAnnotations;

namespace Tz.DataObjects
{
    public class ProductDataObject:DataObjectBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(100, ErrorMessage = "长度超出限制")]
        public string Title { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public Guid BrandCD { get; set; }
        /// <summary>
        /// 类别ID
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 折扣价
        /// </summary>
        public double RebatePrice { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public string ShelfLife { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ProductContent { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        public string SEOTitle { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public string SEOKeyWord { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        public string SEODescription { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string SourceFrom { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}