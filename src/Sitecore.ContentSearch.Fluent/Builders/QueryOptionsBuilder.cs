// <copyright file="SearchQueryOptionsBuilder.cs" company="Kyle Kingsbury">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Linq.Utilities;
    using Options;
    using Results;

    /// <summary>
    /// SearchQueryOptionsBuilder Summary
    /// </summary>
    public class QueryOptionsBuilder<T> where T : SearchResultItem
    {        
        /// <summary>
        /// Gets or sets the QueryOptions
        /// </summary>
        protected QueryOptions<T> QueryOptions { get; set; }

        public QueryOptionsBuilder(QueryOptions<T> queryOptions)
        {
            this.QueryOptions = queryOptions;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public QueryOptionsBuilder<T> And(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.Filter.And(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results if available
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public QueryOptionsBuilder<T> Or(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>(false);

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.Filter.Or(searchOptions.Filter);

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
        }
    }
}