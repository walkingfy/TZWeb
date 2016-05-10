using System;
using System.ComponentModel.DataAnnotations;

using Tz.Core.Validations;

namespace Tz.Domain
{
    public abstract class AggregateRoot :EntityBase,IAggregateRoot
    {
        /// <summary>
        /// 版本号(乐观锁)
        /// </summary>
        public byte[] Version { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="results"></param>
        protected override void Validate(ValidationResultCollection results)
        {
            if (Id == Guid.Empty)
            {
                results.Add(new ValidationResult("Id不能为空"));
            }
        }
    }
}
