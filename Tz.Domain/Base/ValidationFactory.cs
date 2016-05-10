
using Tz.Core.Validations;

namespace Tz.Domain
{
    public class ValidationFactory
    {
        /// <summary>
        /// 创建验证操作
        /// </summary>
        /// <returns></returns>
        public static IValidation Create()
        {
            return new Validation();
        }
    }
}
