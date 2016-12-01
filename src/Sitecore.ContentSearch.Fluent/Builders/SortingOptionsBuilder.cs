// <copyright file="SortingOptionsBuilder.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Builders
{
    using System;
    using System.Linq.Expressions;
    using Options;
    using Results;

    /// <summary>
    /// Builder for creating the Sorting Options
    /// </summary>
    public class SortingOptionsBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// The SortingOptions
        /// </summary>
        protected readonly SortingOptions<T> SortingOptions;

        public SortingOptionsBuilder(SortingOptions<T> sortingOptions)
        {
            this.SortingOptions = sortingOptions;
        }

        /// <summary>
        /// Adds a sorting expression to the Query Options
        /// </summary>
        /// <param name="expression">Expression to apply to the queryable</param>
        /// <returns>Instance of the SortingOptionsBuilder</returns>
        public SortingOptionsBuilder<T> By(Expression<Func<T, object>> expression)
        {
            return this.By(expression, SortOrder.Ascending);
        }

        /// <summary>
        /// Adds a sorting expression to the Query Options
        /// </summary>
        /// <param name="expression">Expression to apply to the queryable</param>
        /// <param name="sortOrder">Sorting Order</param>
        /// <returns>Instance of the SortingOptionsBuilder</returns>
        public SortingOptionsBuilder<T> By(Expression<Func<T, object>> expression, SortOrder sortOrder)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), "Sort Expression cannot be null");
            }

            this.SortingOptions.Expressions.Add(new SortingOptions<T>.SortingOperation(sortOrder, expression));

            return this;
        }


    }
}
