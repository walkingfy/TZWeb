using System;
using System.Linq;
using Tz.Core.Exceptions;

namespace Tz.Core.Validations
{
    /// <summary>
    /// 默认验证处理器，直接抛出异常
    /// </summary>
    public class ValidationHandler : IValidationHandler
    {
        public void Handle(ValidationResultCollection results)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 处理验证错误
        /// </summary>
        /// <param name="results">验证结果集</param>
        public void Handler(ValidationResultCollection results)
        {
            if (results.IsValid)
                return;
            throw new CustomException(results.First().ErrorMessage);
        }
    }
}
