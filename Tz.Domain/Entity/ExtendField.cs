using Tz.Domain.ValueObject;

namespace Tz.Domain.Entity
{
    public class ExtendField : AggregateRoot
    {
        public ExtendField()
        { }

        #region Public Properties
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public EnumIsVisible IsVisible { get; set; }

        #endregion
    }
}