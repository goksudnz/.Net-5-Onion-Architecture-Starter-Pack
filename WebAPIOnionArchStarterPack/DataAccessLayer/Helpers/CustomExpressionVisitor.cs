// Copyrights(c) Charqe.io. All rights reserved.

using System.Linq.Expressions;

namespace DataAccessLayer.Helpers
{
    /// <summary>
    /// To handle expression visits by lambda expressions.
    /// </summary>
    internal class CustomExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Target parameter expression.
        /// </summary>
        private ParameterExpression Target { get; }
        
        /// <summary>
        /// Replacement parameter expression.
        /// </summary>
        private ParameterExpression Replacement { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Right expression.</param>
        /// <param name="replacement">Left expression.</param>
        internal CustomExpressionVisitor(ParameterExpression target, ParameterExpression replacement)
        {
            Target = target;
            Replacement = replacement;
        }

        /// <summary>
        /// Check parameters and returns replacement expression.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node) => node == Target ? Replacement : base.VisitParameter(node);
    }
}