using System;

namespace Tz.Domain.Entity
{
    public class ImageFile : AggregateRoot
    {
         public ImageFile()
        { }

        #region Public Properties
        /// <summary>
        /// 图片文件
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 图片缩略图
        /// </summary>
        public string ImageThumbUrl { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 图片所属类别
        /// </summary>
        public int ImageType { get; set; }
        #endregion
    }
}