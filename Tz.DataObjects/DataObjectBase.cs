using System;
using System.Text;
using Tz.Core.Validations;

namespace Tz.DataObjects
{
    public class DataObjectBase
    {
        public Guid Id { get; set; }

        public string ErrorString { get; set; }

        public virtual bool Validate()
        {
            var result = GetValidationResult();
            var resultBuilder=new StringBuilder();
            foreach (var item in result)
            {
                resultBuilder.Append("● " + item.ErrorMessage + "<br/>");
            }
            ErrorString = resultBuilder.ToString();
            return result.IsValid;
        }
        /// <summary>
        /// 获取验证结果
        /// </summary>
        /// <returns></returns>
        private ValidationResultCollection GetValidationResult()
        {
            return ValidationFactory.Create().Validate(this);
        }
    }
}