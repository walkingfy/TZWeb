using System.Collections.Generic;
using System.Text;
using Tz.Core.Validations;

namespace Tz.Domain
{
    public class DomainBase
    {

        #region 构造方法

        protected DomainBase()
        {
            _rules = new List<IValidationRule>();
            _handler = new ValidationHandler();
        }
        #endregion

        #region 字段
        /// <summary>
        /// 描述
        /// </summary>
        private StringBuilder _description;
        /// <summary>
        /// 验证规则集合
        /// </summary>
        private readonly List<IValidationRule> _rules;
        /// <summary>
        /// 验证处理器
        /// </summary>
        private IValidationHandler _handler;
        #endregion

        #region ToString(输出领域对象的状态)
        /// <summary>
        /// 输出领域对象的状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            _description = new StringBuilder();
            AddDescription();
            return _description.ToString().TrimEnd().TrimEnd(',');
        }
        /// <summary>
        /// 添加描述
        /// </summary>
        protected virtual void AddDescription()
        { }
        /// <summary>
        /// 添加描述
        /// </summary>
        /// <param name="description"></param>
        protected void AddDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return;
            _description.Append(description);
        }
        /// <summary>
        /// 添加描述
        /// </summary>
        protected void AddDescription<T>(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return;
            _description.AppendFormat("{0}:{1},", name, value);
        }
        #endregion

        #region SetValidationHandler(设置验证处理器)
        /// <summary>
        /// 设置验证处理器
        /// </summary>
        /// <param name="handler">验证处理器</param>
        public void SetValidationHandler(IValidationHandler handler)
        {
            if (handler == null)
            {
                return;
            }
            _handler = handler;
        }
        #endregion

        #region AddValidationRule(添加验证规则)
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        public void AddValidationRule(IValidationRule rule)
        {
            if (rule == null)
            {
                return;
            }
            _rules.Add(rule);
        }
        #endregion

        #region Validation(验证)
        /// <summary>
        /// 验证
        /// </summary>
        public virtual void Validate()
        {
            var result = GetValidationResult();
            HandleValidationResult(result);
        }

        /// <summary>
        /// 获取验证结果
        /// </summary>
        /// <returns></returns>
        public ValidationResultCollection GetValidationResult()
        {
            var result = ValidationFactory.Create().Validate(this);
            Validate(result);
            foreach (var rule in _rules)
            {
                result.Add(rule.Validate());
            }
            return result;
        }

        /// <summary>
        /// 验证并添加到验证结果集合
        /// </summary>
        /// <param name="results">验证结果集合</param>
        protected virtual void Validate(ValidationResultCollection results)
        {

        }

        /// <summary>
        /// 处理验证结果
        /// </summary>
        /// <param name="results"></param>
        private void HandleValidationResult(ValidationResultCollection results)
        {
            if (results.IsValid)
            {
                return;
            }
            _handler.Handle(results);
        }

        #endregion
    }
}
