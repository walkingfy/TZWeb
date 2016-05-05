using System;
using System.Linq.Expressions;

namespace Tz.Domain.Specifications
{
    public class ExpressionSpecification<T> : Specification<T>
    {
        #region Private Fields
        private Expression<Func<T, bool>> expression;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>ExpressionSpecification&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="expression">The LINQ expression which represents the current
        /// specification.</param>
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            this.expression = expression;
        }
        #endregion

        #region Public Methods

        public override Expression<Func<T, bool>> GetExpression()
        {
            return this.expression;
        }
        #endregion
    }
}
