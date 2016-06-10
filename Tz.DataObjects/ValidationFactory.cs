using Tz.Core.Validations;

namespace Tz.DataObjects
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