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
namespace Sitecore.ContentSearch.Fluent.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;
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
        /// Adds each key as a sort directly
        /// </summary>
        /// <param name="sorting">The sortings</param>
        /// <param name="sortOrderExpression">The expression to determine which the order</param>
        /// <returns>Instance of the SortingOptionsBuilder</returns>
        public virtual SortingOptionsBuilder<T> ByAll(IEnumerable<KeyValuePair<string, string>> sorting,  Func<string, SortOrder> sortOrderExpression)
        {
            var keyValuePairs = sorting as KeyValuePair<string, string>[] ?? sorting.ToArray();

            foreach (var pair in keyValuePairs)
            {
                this.SortingOptions.Operations.Add(new SortingOptions<T>.SortingOperation(sortOrderExpression(pair.Value), (result => result[pair.Key])));
            }

            return this;
        }

        /// <summary>
        /// Adds a sorting expression to the Query Options
        /// </summary>
        /// <param name="expression">Expression to apply to the queryable</param>
        /// <param name="sortOrder">Sorting Order</param>
        /// <exception cref="ArgumentNullException">Expression cannot be null</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a method</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a field</exception>
        /// <returns>Instance of the SortingOptionsBuilder</returns>
        public virtual SortingOptionsBuilder<T> By(Expression<Func<T, object>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), "Sort Expression cannot be null");
            }

            /*var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");
            }*/
            
            this.SortingOptions.Operations.Add(new SortingOptions<T>.SortingOperation(sortOrder, expression));

            return this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="By"/> sorting to the current expression tree
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <param name="expression">The sorting expression</param>
        /// <param name="sortOrder">The sort order. Defaults to <see cref="SortOrder.Ascending"/></param>
        /// <returns><see cref="SortingOptionsBuilder{T}"/></returns>
        public virtual SortingOptionsBuilder<T> IfBy(bool condition, Expression<Func<T, object>> expression, SortOrder sortOrder = SortOrder.Ascending)
        {
            return condition ? this.By(expression, sortOrder) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, for each key will add the resulting sorting to the current expression tree using <see cref="ByAll"/>
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <param name="sorting">The sorting keys</param>
        /// <param name="sortOrderExpression">The sort order. Defaults to <see cref="SortOrder.Ascending"/></param>
        /// <returns><see cref="SortingOptionsBuilder{T}"/></returns>
        public virtual SortingOptionsBuilder<T> IfByAll(bool condition, IEnumerable<KeyValuePair<string, string>> sorting, Func<string, SortOrder> sortOrderExpression)
        {
            return condition ? this.ByAll(sorting, sortOrderExpression) : this;
        }
    }
}