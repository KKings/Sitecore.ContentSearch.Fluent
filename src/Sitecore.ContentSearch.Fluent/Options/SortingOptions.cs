// <copyright file="SortingOptions.cs" company="Kyle Kingsbury">
//  Copyright 2015 Kyle Kingsbury
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.

//  You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an 'AS IS' BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// </copyright>
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
        /// Gets or sets the Expressions
        /// </summary>
        public IList<SortingOperation> Expressions { get; set; }

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
            if (this.Expressions.Any())
            {
                // Resolve bug with Sitecore not evaluating orders correctly
                // http://www.daveleigh.co.uk/sitecore-content-search-api-thenby-clause-not-evaluating-correctly/
                var expressions = Expressions.Reverse();

                var sortingOperations = expressions as SortingOperation[] ?? expressions.ToArray();

                var orderByExpression = sortingOperations.First();

                var orderedQueryable = (orderByExpression.SortOrder == SortOrder.Ascending)
                    ? queryable.OrderBy(orderByExpression.Expression)
                    : queryable.OrderByDescending(orderByExpression.Expression);

                return sortingOperations.Skip(1)
                    .Aggregate(orderedQueryable,
                        (current, expression) =>
                            (expression.SortOrder == SortOrder.Ascending)
                                ? current.ThenBy(expression.Expression)
                                : current.ThenByDescending(expression.Expression));
            }

            return queryable;
        }

        /// <summary>
        /// Stores Sorting Operations
        /// </summary>
        public class SortingOperation
        {
            /// <summary>
            /// Gets or sets the Sort Order
            /// </summary>
            public SortOrder SortOrder { get; set; }

            /// <summary>
            /// Get or sets the Expression
            /// </summary>
            public Expression<Func<T, object>> Expression { get; set; }

            public SortingOperation(SortOrder sortOrder, Expression<Func<T, object>> expression)
            {
                this.SortOrder = sortOrder;
                this.Expression = expression;
            }
        }
    }
}
