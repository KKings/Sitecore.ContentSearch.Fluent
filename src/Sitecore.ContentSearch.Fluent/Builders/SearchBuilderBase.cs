// <copyright file="FilterOptions.cs" company="Kyle Kingsbury">
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
    using Expressions;
    using Extensions;
    using Linq.Utilities;
    using Options;
    using Results;

    public abstract class SearchBuilderBase<T> where T : SearchResultItem
    {
        internal virtual QueryableOptions<T> Options { get; }

        protected SearchBuilderBase(QueryableOptions<T> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));    
            }

            this.Options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterAction"></param>
        /// <returns></returns>
        public abstract SearchBuilderBase<T> And(Action<SearchBuilderBase<T>> filterAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterAction"></param>
        /// <returns></returns>
        public abstract SearchBuilderBase<T> Or(Action<SearchBuilderBase<T>> filterAction);

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public SearchBuilderBase<T> Not(Action<SearchQueryBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            this.Options.Filter = this.Options.Filter != null
                ? this.Options.Filter.And(searchOptions.Filter.Not())
                : PredicateBuilder.True<T>().And(searchOptions.Filter.Not());

            return this;
        }
                
        public SearchBuilderBase<T> Where(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                this.Options.Filter = this.Options.Filter != null 
                    ? this.Options.Filter.And(filter)
                    : PredicateBuilder.True<T>().And(filter);
            }

            return this;
        }
        
        public SearchBuilderBase<T> OrWhere(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                this.Options.Filter = this.Options.Filter != null 
                    ? this.Options.Filter.Or(filter) 
                    : PredicateBuilder.False<T>().Or(filter);
            }

            return this;
        }

        public SearchBuilderBase<T> All<TR>(IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            var enumerable = terms as TR[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.True<T>();

                predicate =
                    enumerable
                        .Select(filter.Rewrite)
                        .Aggregate(predicate, (current, expression) => current.And(expression));

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.And(predicate)
                    : PredicateBuilder.True<T>().And(predicate);
            }

            return this;
        }

        public SearchBuilderBase<T> OrAll<TR>(IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            var enumerable = terms as TR[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.True<T>();

                predicate =
                    enumerable
                        .Select(filter.Rewrite)
                        .Aggregate(predicate, (current, expression) => current.And(expression));

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.Or(predicate)
                    : PredicateBuilder.False<T>().Or(predicate);
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchBuilderBase<T> Any<TR>(IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            var enumerable = terms as TR[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.False<T>();

                predicate =
                    enumerable
                        .Select(filter.Rewrite)
                        .Aggregate(predicate, (current, expression) => current.Or(expression));

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.And(predicate)
                    : PredicateBuilder.True<T>().And(predicate);
            }

            return this;
        }

        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchBuilderBase<T> OrAny<TR>(IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            var enumerable = terms as TR[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.False<T>();

                predicate =
                    enumerable
                        .Select(filter.Rewrite)
                        .Aggregate(predicate, (current, expression) => current.Or(expression));

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.Or(predicate)
                    : PredicateBuilder.False<T>().Or(predicate);
            }

            return this;
        }

        public SearchBuilderBase<T> IfAny<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.Any(terms, filter) : this;
        }

        public SearchBuilderBase<T> IfOrAny<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.OrAny(terms, filter) : this;
        }

        public SearchBuilderBase<T> IfAll<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.All(terms, filter) : this;
        }

        public SearchBuilderBase<T> IfOrAll<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.OrAll(terms, filter) : this;
        }

        public SearchBuilderBase<T> IfWhere(bool condition, Expression<Func<T, bool>> filter)
        {
            return condition ? this.Where(filter) : this;
        }

        public virtual SearchBuilderBase<T> IfOrWhere(bool condition, Expression<Func<T, bool>> filter)
        {
            return condition ? this.OrWhere(filter) : this;
        }

        /*
        /// <summary>
        /// Filters out the Search Results by aggregating an array.
        /// <para>Passes each array item with a predicate into the expression for the caller</para>
        /// </summary>
        /// <param name="terms">IEnumerable to aggregate on</param>
        /// <param name="filter">Lambda expression to filter on</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearchBuilderBase<T> OrWhere<TR>(IEnumerable<TR> terms,
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter)
        {
            var array = terms as TR[] ?? terms.ToArray();

            if (array.Any() && filter != null)
            {
                this.Options.Filter = this.Options.Filter.Or(array.Aggregate(PredicateBuilder.True<T>(), filter));
            }

            return this;
        }*/
        /*
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
        public SearchQueryBuilder<T> WhereLoop<TR>(IEnumerable<IEnumerable<TR>> terms,
            Func<Expression<Func<T, bool>>, TR, Expression<Func<T, bool>>> filter,
            bool outtPredicateOr = false,
            bool innerPredicateOr = false)
        {
            var enumerable = terms as IEnumerable<TR>[] ?? terms.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var seedPredicate = outtPredicateOr ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                foreach (var group in enumerable)
                {
                    var innerPredicate = innerPredicateOr ? PredicateBuilder.False<T>() : PredicateBuilder.True<T>();

                    seedPredicate = seedPredicate.And(group.Aggregate(innerPredicate, filter));
                }

                this.QueryOptions.Filter = this.QueryOptions.Filter.And(seedPredicate);
            }

            return this;
        }*/
    }
}
