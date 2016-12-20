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
        /// <param name="sortOrder">Sorting Order</param>
        /// <returns>Instance of the SortingOptionsBuilder</returns>
        public SortingOptionsBuilder<T> By(Expression<Func<T, object>> expression, SortOrder sortOrder = SortOrder.Ascending)
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