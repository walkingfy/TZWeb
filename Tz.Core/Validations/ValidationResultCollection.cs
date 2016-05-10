/*
*Author:zelin
*CreateDate:2015-10-6 01:30:03
*/
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tz.Core.Validations
{
    public class ValidationResultCollection : IEnumerable<ValidationResult>
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        private readonly List<ValidationResult> _results;

        /// <summary>
        /// 初始化验证结果集
        /// </summary>
        public ValidationResultCollection()
        {
            _results = new List<ValidationResult>();
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _results.Count == 0;
            }
        }

        /// <summary>
        /// 验证结果个数
        /// </summary>
        public int Count
        {
            get
            {
                return _results.Count;
            }
        }

        /// <summary>
        /// 添加验证结果
        /// </summary>
        /// <param name="result">验证结果</param>
        public void Add(ValidationResult result)
        {
            if (result == null)
                return;
            _results.Add(result);
        }

        /// <summary>
        /// 添加验证结果集合
        /// </summary>
        /// <param name="results">验证结果集合</param>
        public void AddRange(IEnumerable<ValidationResult> results)
        {
            if (results == null)
                return;
            foreach (var result in results)
                Add(result);
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator()
        {
            return _results.GetEnumerator();
        }
    }
}
