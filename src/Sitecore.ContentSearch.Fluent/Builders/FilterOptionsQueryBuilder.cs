// <copyright file="FilterOptionsQueryBuilder.cs" company="Kyle Kingsbury">
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
    using global::Sitecore.ContentSearch.Linq.Utilities;
    using Options;
    using Results;

    /// <summary>
    /// Todo: FilterOptionsQueryBuilder Summary Description
    /// </summary>
    public class FilterOptionsQueryBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the QueryOptions
        /// </summary>
        protected FilterOptions<T> FilterOptions { get; set; }

        public FilterOptionsQueryBuilder()
        {
            this.FilterOptions = new FilterOptions<T>();
        }

        public FilterOptionsQueryBuilder(FilterOptions<T> filterOptions)
        {
            this.FilterOptions = filterOptions;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> And(Action<FilterOptionsQueryBuilder<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>();

            filterAction(new FilterOptionsQueryBuilder<T>(filterOptions));

            this.FilterOptions.Filter = this.FilterOptions.Filter.And(filterOptions.Filter);

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> Or(Action<FilterOptionsQueryBuilder<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>(false);

            filterAction(new FilterOptionsQueryBuilder<T>(filterOptions));

            this.FilterOptions.Filter = this.FilterOptions.Filter.Or(filterOptions.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> Where(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                this.FilterOptions.Filter = this.FilterOptions.Filter.And(filter);
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results if condition is met
        /// </summary>
        /// <param name="condition">Applies the filter only if condition is met</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> Where(bool condition, Expression<Func<T, bool>> filter)
        {
            if (condition && filter != null)
            {
                this.FilterOptions.Filter = this.FilterOptions.Filter.And(filter);
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
        /// <param name="isPredicateOr">Switches the PredicateBuilder Predicate Seed</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> Where(string terms,
            Func<Expression<Func<T, bool>>, string, Expression<Func<T, bool>>> filter,
            bool isPredicateOr = false)
        {
            if (!String.IsNullOrEmpty(terms) && filter != null)
            {
                var termsSplit = terms.Split(new char[' '], StringSplitOptions.RemoveEmptyEntries).ToArray();

                if (termsSplit.Any())
                {
                    var termPredicate = (isPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                    this.FilterOptions.Filter =
                        this.FilterOptions.Filter.And(termsSplit.Aggregate(termPredicate, filter));
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
        public FilterOptionsQueryBuilder<T> Where<TR>(IEnumerable<TR> terms,
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter,
            bool isPredicateOr = false)
        {
            var enumerable = terms as TR[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var termPredicate = (isPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                this.FilterOptions.Filter = this.FilterOptions.Filter.And(enumerable.Aggregate(termPredicate, filter));
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
        /// <param name="innerPredicateOr">Switches the Internal PredicateBuilder Predicate Seed</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public FilterOptionsQueryBuilder<T> WhereLoop<TR>(IEnumerable<IEnumerable<TR>> terms,
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter,
            bool innerPredicateOr = false)
        {
            var enumerable = terms as IEnumerable<TR>[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                foreach (var group in enumerable)
                {
                    var innerPredicate = (innerPredicateOr) ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                    this.FilterOptions.Filter = this.FilterOptions.Filter.And(group.Aggregate(innerPredicate, filter));
                }
            }

            return this;
        }
    }
}
