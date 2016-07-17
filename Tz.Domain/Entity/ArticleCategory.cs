using System;
using System.ComponentModel.DataAnnotations;

namespace Tz.Domain.Entity
{
    public class ArticleCategory : AggregateRoot
    {
        public ArticleCategory()
        {
        }

        #region Public Properties
        /// <summary>
        /// 类别标题
        /// </summary>
        [MaxLength(255,ErrorMessage = "长度超出限制")]
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        [MaxLength(255,ErrorMessage = "长度超出限制")]
        public string Description { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public Guid ParentId { get; set; }


        #endregion
    }
}