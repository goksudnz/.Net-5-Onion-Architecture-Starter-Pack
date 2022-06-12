// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DataAccessLayer.Helpers
{
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// AndAlso expression extension.
        /// </summary>
        /// <param name="left">Left expression.</param>
        /// <param name="right">Right expression.</param>
        /// <typeparam name="T1">Left expression object type.</typeparam>
        /// <typeparam name="T2">Right expression object type.</typeparam>
        /// <returns>Composed expression of left and right expressions.</returns>
        internal static Expression<Func<T1, bool>> And<T1, T2>(this Expression<Func<T1, bool>> left, Expression<Func<T2, bool>> right)
        {
            var visitor = new CustomExpressionVisitor(right.Parameters[0], left.Parameters[0]);
            var rewrittenRight = visitor.Visit(right.Body);
            var andExpression = Expression.AndAlso(left.Body, rewrittenRight);

            return Expression.Lambda<Func<T1, bool>>(andExpression, left.Parameters);
        }

        /// <summary>
        /// OrElse expression extension.
        /// </summary>
        /// <param name="left">Left expression.</param>
        /// <param name="right">Right expression.</param>
        /// <typeparam name="T1">Left expression object type.</typeparam>
        /// <typeparam name="T2">Right expression object type.</typeparam>
        /// <returns>Composed expression of left and right expressions.</returns>
        internal static Expression<Func<T1, bool>> Or<T1, T2>(this Expression<Func<T1, bool>> left, Expression<Func<T2, bool>> right)
        {
            var visitor = new CustomExpressionVisitor(right.Parameters[0], left.Parameters[0]);
            var rewrittenRight = visitor.Visit(right.Body);
            var orExpression = Expression.OrElse(left.Body, rewrittenRight);

            return Expression.Lambda<Func<T1, bool>>(orExpression, left.Parameters);
        }
    }
}