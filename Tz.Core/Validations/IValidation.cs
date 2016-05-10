namespace Tz.Core.Validations
{
    /// <summary>
    /// 验证接口
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target">验证目标</param>
        /// <returns></returns>
        ValidationResultCollection Validate(object target);
    }
}
