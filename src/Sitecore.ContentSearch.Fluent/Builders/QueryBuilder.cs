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
    /// SearchQueryOptionsBuilder Summary
    /// </summary>
    public class QueryBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the QueryOptions
        /// </summary>
        protected readonly QueryOptions<T> QueryOptions;

        public QueryBuilder(QueryOptions<T> queryOptions)
        {
            this.QueryOptions = queryOptions;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public QueryBuilder<T> And(Action<SearchQueryBuilder<T>> filterAction)
        {
            if (filterAction == null)
            {
                throw new ArgumentNullException(nameof(filterAction));
            }

            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            if (searchOptions.Filter == null)
            {
                return this;
            }

            this.QueryOptions.Filter = this.QueryOptions.Filter != null
                ? this.QueryOptions.Filter.And(searchOptions.Filter) /*this.QueryOptions.Filter.CombineWithAndAlso(searchOptions.Filter)*/
                : PredicateBuilder.True<T>().And(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public QueryBuilder<T> Or(Action<SearchQueryBuilder<T>> filterAction)
        {
            if (filterAction == null)
            {
                throw new ArgumentNullException(nameof(filterAction));
            }

            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            if (searchOptions.Filter == null)
            {
                return this;
            }

            this.QueryOptions.Filter = this.QueryOptions.Filter != null
                ? this.QueryOptions.Filter.Or(searchOptions.Filter) //this.QueryOptions.Filter.CombineWithOrElse(searchOptions.Filter)
                : PredicateBuilder.False<T>().Or(searchOptions.Filter);

            return this;
        }
        /*
                /// <summary>
                /// Filters out the Search Results
                /// </summary>
                /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> AndOr(Action<SearchQueryBuilder<T>> filterAction)
                {
                    var searchOptions = new QueryOptions<T>(true, false);

                    filterAction(new SearchQueryBuilder<T>(searchOptions));

                    this.QueryOptions.Filter = this.QueryOptions.Filter.And(searchOptions.Filter);

                    return this;
                }

                /// <summary>
                /// Filters out the Search Results if available
                /// </summary>
                /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> Or(Action<SearchQueryBuilder<T>> filterAction)
                {
                    var searchOptions = new QueryOptions<T>(false);

                    filterAction(new SearchQueryBuilder<T>(searchOptions));

                    this.QueryOptions.Filter = this.QueryOptions.Filter.And(searchOptions.Filter);

                    return this;
                }

                /// <summary>
                /// Filters out the Search Results. Filters with Relevancy and Scoring.
                /// </summary>
                /// <param name="filter">Lambda expression to filter on</param>
                /// <returns>Instance of the QueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> Where(Expression<Func<T, bool>> filter)
                {
                    if (filter != null)
                    {
                        this.QueryOptions.Queryable = this.QueryOptions.Queryable.Where(filter);
                    }

                    return this;
                }

                /// <summary>
                /// Filters out the Search Results. Filters with Relevancy and Scoring.
                /// </summary>
                /// <param name="filter">Lambda expression to filter on</param>
                /// <param name="condition"></param>
                /// <returns>Instance of the QueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> Where(bool condition, Expression<Func<T, bool>> filter)
                {
                    if (condition && filter != null)
                    {
                        this.QueryOptions.Queryable = this.QueryOptions.Queryable.Where(filter);
                    }

                    return this;
                }

                /// <summary>
                /// Filters out the Search Results by aggregating an array. Splits the terms 
                /// into an array to aggregate on.
                /// <para>Passes each array item with a predicate into the expression for the caller</para>
                /// </summary>
                /// <param name="terms">Terms to split and aggregate on</param>
                /// <param name="filter">Lambda expression to filter on</param>
                /// <returns>Instance of the QueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> Where(string terms, Func<IQueryable<T>, string, IQueryable<T>> filter)
                {
                    if (!String.IsNullOrEmpty(terms) && filter != null)
                    {
                        var termsSplit = terms.Split(new []{ ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                        return Any(termsSplit, filter);
                    }

                    return this;
                }

                /// <summary>
                /// Filters out the Search Results by aggregating an array. Splits the terms 
                /// into an array to aggregate on.
                /// <para>Passes each array item with a predicate into the expression for the caller</para>
                /// </summary>
                /// <param name="terms">Terms to split and aggregate on</param>
                /// <param name="filter">Lambda expression to filter on</param>
                /// <returns>Instance of the QueryOptionsBuilder</returns>
                public QueryOptionsBuilder<T> Any<TR>(IList<TR> terms, Func<IQueryable<T>, TR, IQueryable<T>> filter)
                {
                    if (terms.Any())
                    {
                        this.QueryOptions.Queryable = terms.Aggregate(this.QueryOptions.Queryable, filter);
                    }

                    return this;
                }*/
    }
}