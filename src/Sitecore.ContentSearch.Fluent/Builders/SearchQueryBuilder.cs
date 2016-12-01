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
    using Linq.Utilities;
    using Options;
    using Results;

    /// <summary>
    /// SearchQueryBuilder Summary
    /// </summary>
    public class SearchQueryBuilder<T> : SearchBuilderBase<T> where T : SearchResultItem
    {
        public SearchQueryBuilder() : this(new QueryOptions<T>()) { }

        public SearchQueryBuilder(QueryOptions<T> queryOptions) : base(queryOptions) { }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public override SearchBuilderBase<T> And(Action<SearchBuilderBase<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            this.Options.Filter = this.Options.Filter != null
                ? this.Options.Filter.And(searchOptions.Filter)
                : PredicateBuilder.True<T>().And(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public override SearchBuilderBase<T> Or(Action<SearchBuilderBase<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            this.Options.Filter = this.Options.Filter != null
                ? this.Options.Filter.Or(searchOptions.Filter)
                : PredicateBuilder.False<T>().Or(searchOptions.Filter);

            return this;
        }

    }
}