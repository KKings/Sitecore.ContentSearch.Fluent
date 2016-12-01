// MIT License
// 
// Copyright (c) 2016 Kyle Kingsbury
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace Sitecore.ContentSearch.Fluent.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Results;

    /// <summary>
    /// Stores sorting options
    /// </summary>
    public class SortingOptions<T> where T : SearchResultItem
    {
        /// <summary>
        /// Stores Sorting Operations
        /// </summary>
        internal class SortingOperation
        {
            /// <summary>
            /// Gets or sets the Sort Order
            /// </summary>
            public virtual SortOrder SortOrder { get; }

            /// <summary>
            /// Get or sets the Expression
            /// </summary>
            public virtual Expression<Func<T, object>> Expression { get; }

            public SortingOperation(SortOrder sortOrder, Expression<Func<T, object>> expression)
            {
                this.SortOrder = sortOrder;
                this.Expression = expression;
            }
        }

        /// <summary>
        /// Gets or sets the Expressions
        /// </summary>
        internal IList<SortingOperation> Expressions { get; set; }

        public SortingOptions()
        {
            this.Expressions = new List<SortingOperation>();
        }

        /// <summary>
        /// Apply the sortings to an IQueryable
        /// </summary>
        /// <param name="queryable">Current Search IQueryable</param>
        /// <returns>IQueryable with Sortings Applied</returns>
        internal IQueryable<T> ApplySorting(IQueryable<T> queryable)
        {
            if (!this.Expressions.Any())
            {
                return queryable;
            }

            // Resolve bug with Sitecore not evaluating orders correctly
            // http://www.daveleigh.co.uk/sitecore-content-search-api-thenby-clause-not-evaluating-correctly/
            var expressions = this.Expressions.Reverse();

            var sortingOperations = expressions as SortingOperation[] ?? expressions.ToArray();

            var orderByExpression = sortingOperations.First();

            var orderedQueryable = orderByExpression.SortOrder == SortOrder.Ascending
                ? queryable.OrderBy(orderByExpression.Expression)
                : queryable.OrderByDescending(orderByExpression.Expression);

            return sortingOperations.Skip(1)
                                    .Aggregate(orderedQueryable,
                                        (current, expression) =>
                                            expression.SortOrder == SortOrder.Ascending
                                                ? current.ThenBy(expression.Expression)
                                                : current.ThenByDescending(expression.Expression));
        }
    }
}
