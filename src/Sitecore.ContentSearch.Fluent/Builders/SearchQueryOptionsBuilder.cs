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
    public class SearchQueryOptionsBuilder<T> where T : SearchResultItem
    {      
        /// <summary>
        /// Gets or sets the QueryOptions
        /// </summary>
        protected QueryOptions<T> QueryOptions { get; set; }

        public SearchQueryOptionsBuilder()
        {
            this.QueryOptions = new QueryOptions<T>();
        }

        public SearchQueryOptionsBuilder(QueryOptions<T> queryOptions)
        {
            this.QueryOptions = queryOptions;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> And(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.UseAndPredicate 
                ? this.QueryOptions.Filter.And(searchOptions.Filter) 
                : this.QueryOptions.Filter.Or(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Or(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>(false);

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.UseAndPredicate
                ? this.QueryOptions.Filter.And(searchOptions.Filter)
                : this.QueryOptions.Filter.Or(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Not(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.Filter.And(searchOptions.Filter.Not());

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> AndOr(Action<SearchQueryOptionsBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>(true, false);

            filterAction(new SearchQueryOptionsBuilder<T>(searchOptions));

            this.QueryOptions.Filter = this.QueryOptions.UseAndPredicate
                ? this.QueryOptions.Filter.And(searchOptions.Filter)
                : this.QueryOptions.Filter.Or(searchOptions.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Where(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                this.QueryOptions.Filter = this.QueryOptions.UseAndPredicate ? this.QueryOptions.Filter.And(filter) : this.QueryOptions.Filter.Or(filter);
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results if condition is met
        /// </summary>
        /// <param name="condition">Applies the filter only if condition is met</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Where(bool condition, Expression<Func<T, bool>> filter)
        {
            if (condition && filter != null)
            {
                this.QueryOptions.Filter = this.QueryOptions.UseAndPredicate ? this.QueryOptions.Filter.And(filter) : this.QueryOptions.Filter.Or(filter);
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
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Where(string terms, 
            Func<Expression<Func<T, bool>>, string, Expression<Func<T, bool>>> filter)
        {
            if (!String.IsNullOrEmpty(terms) && filter != null)
            {
                var termsSplit = terms.Split(new char[' '], StringSplitOptions.RemoveEmptyEntries).ToArray();

                if (termsSplit.Any())
                {
                    var termPredicate = PredicateBuilder.True<T>();

                    this.QueryOptions.Filter = this.QueryOptions.Filter.And(termsSplit.Aggregate(termPredicate, filter));
                }
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <param name="isPredicateOr">Switches the PredicateBuilder Predicate Seed</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Where(IEnumerable<string> terms,
            Func<Expression<Func<T, bool>>, string, Expression<Func<T, bool>>> filter,
            bool isPredicateOr = false)
        {
            var enumerable = terms as string[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var termPredicate = (isPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                this.QueryOptions.Filter = this.QueryOptions.Filter.And(enumerable.Aggregate(termPredicate, filter));
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <param name="isPredicateOr">Switches the PredicateBuilder Predicate Seed</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> Where<TR>(IEnumerable<TR> terms, 
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter,
            bool isPredicateOr = false)
        {
            var array = terms as TR[] ?? terms.ToArray();

            if (array.Any() && filter != null)
            {
                var termPredicate = (isPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                this.QueryOptions.Filter = this.QueryOptions.Filter.And(array.Aggregate(termPredicate, filter));
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// <para>
        /// TODO: Think about supporting (n) of layers
        /// </para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <param name="outtPredicateOr"></param>
        /// <param name="innerPredicateOr">Switches the Internal PredicateBuilder Predicate Seed</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchQueryOptionsBuilder<T> WhereLoop<TR>(IEnumerable<IEnumerable<TR>> terms,
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter,
            bool outtPredicateOr = false,
            bool innerPredicateOr = false)
        {
            var enumerable = terms as IEnumerable<TR>[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var seedPredicate = (outtPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                foreach (var group in enumerable)
                {
                    var innerPredicate = (innerPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                    seedPredicate = seedPredicate.And(group.Aggregate(innerPredicate, filter));
                }

                this.QueryOptions.Filter = this.QueryOptions.Filter.And(seedPredicate);
            }

            return this;
        }
    }
}