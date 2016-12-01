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
    using Extensions;
    using Linq.Utilities;
    using Options;
    using Results;

    public abstract class SearchBuilderBase<T> where T : SearchResultItem
    {
        /// <summary>
        /// Contains the up-to-date filter at a given context before it is applied
        /// higher in the chain
        /// </summary>
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
        /// Ability to generate child queries using an 'And' operation
        /// Ex. .And(and => and.Where(x).Where(x))
        /// </summary>
        /// <param name="filterAction">Action Expressions</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public abstract SearchBuilderBase<T> And(Action<SearchBuilderBase<T>> filterAction);

        /// <summary>
        /// Ability to generate child queries using an 'Or' operation
        /// Ex. .Or(or => or.Where(x).Where(x))
        /// </summary>
        /// <param name="filterAction">Action Expressions</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public abstract SearchBuilderBase<T> Or(Action<SearchBuilderBase<T>> filterAction);

        /// <summary>
        /// Ability to generate child queries using a 'Not' operation. This negates the queries. In c# you have (x == 1), this will negate it with and be !(x == 1)
        /// Ex. .Not(not => not.Where(x).Where(x))
        /// </summary>
        /// <param name="filterAction">Action Expressions</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> Not(Action<SearchQueryBuilder<T>> filterAction)
        {
            var searchOptions = new QueryOptions<T>();

            filterAction(new SearchQueryBuilder<T>(searchOptions));

            this.Options.Filter = this.Options.Filter != null
                ? this.Options.Filter.And(searchOptions.Filter.Not())
                : PredicateBuilder.True<T>().And(searchOptions.Filter.Not());

            return this;
        }

        /// <summary>
        /// Adds to the expression tree as an 'And' [Current Expression Tree] AND [<see cref="filter"/>]
        /// <para>Example: To filter articles by a specific author, Where(result => result.Author == "Test")
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Author'
        /// </para>
        /// </summary>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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

        /// <summary>
        /// Adds to the expression tree as an 'Or' [Current Expression Tree] OR [<see cref="filter"/>]
        /// <para>Example: To filter articles by a specific author, OrWhere(result => result.Author == "Test")
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Author'
        /// </para>
        /// </summary>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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

        /// <summary>
        /// Expression in which all <see cref="filter"/> need to be true for the entire expression to be true. Each
        /// <see cref="TR"/> generates a new <see cref="filter"/>. So 100 terms, will generate 100 expressions, and all must be true for the entire expression to be true.
        /// <para>Adds to the expression tree as an 'And' [Current Expression Tree] And [<see cref="filter"/>]</para>
        /// <para>Example: To filter articles by tags, All([Tag1, Tag2, Tag3], (result, tag) => result.Tags.Contains(tag))
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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

        /// <summary>
        /// Expression in which all <see cref="filter"/> need to be true for the entire expression to be true. Each
        /// <see cref="TR"/> generates a new <see cref="filter"/>. So 100 terms, will generate 100 expressions, and all must be true for the entire expression to be true.
        /// <para>Adds to the expression tree as an 'Or' [Current Expression Tree] Or [<see cref="filter"/>]</para>
        /// <para>Example: To filter articles by tags, <code>OrAll([Tag1, Tag2, Tag3], (result, tag) => result.Tags.Contains(tag))</code>,
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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
        /// Builds an expression tree by applying a filter expression to each item within a group, then combining each group's expression tree together
        /// <para>Adds to the current expression tree as an 'And' [Current Expression Tree] And [Group 1 Expression And Group 2 Expression] </para>
        /// <para>Each group's expression tree will be combined by using an 'Or', so any term within the group can be matched</para>
        /// <para>Example: To filter articles by tags, <code>ManyAny([[Tag1, Tag2, Tag3], [Tag1, Tag2]], (result, tag) => result.Tags.Contains(tag))</code>,
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="groups">A grouping of terms</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> ManyAny<TR>(IEnumerable<IEnumerable<TR>> groups, Expression<Func<T, TR, bool>> filter)
        { 
            var enumerable = groups as IEnumerable<TR>[] ?? groups.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.True<T>();

                foreach (var group in enumerable)
                {
                    var inner = PredicateBuilder.False<T>();

                    inner 
                        = group
                            .Select(filter.Rewrite)
                            .Aggregate(inner, (current, expression) => current.Or(expression));

                    // Magic here
                    predicate = predicate.And(inner);
                }

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.And(predicate)
                    : PredicateBuilder.True<T>().And(predicate);
            }

            return this;
        }

        /// <summary>
        /// Builds an expression tree by applying a filter expression to each item within a group, then combining each group's expression tree together
        /// <para>Adds to the current expression tree as an 'Or' [Current Expression Tree] Or [Group 1 Expression And Group 2 Expression] </para>
        /// <para>Each group's expression tree will be combined by using an 'Or', so any term within the group can be matched</para>
        /// <para>Example: To filter articles by tags, <code>OrManyAny([[Tag1, Tag2, Tag3], [Tag4, Tag5]], (result, tag) => result.Tags.Contains(tag))</code>,
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="groups">A grouping of terms</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> OrManyAny<TR>(IEnumerable<IEnumerable<TR>> groups, Expression<Func<T, TR, bool>> filter)
        {
            var enumerable = groups as IEnumerable<TR>[] ?? groups.ToArray();

            if (enumerable.Any() && filter != null)
            {
                var predicate = PredicateBuilder.True<T>();

                foreach (var group in enumerable)
                {
                    var inner = PredicateBuilder.False<T>();

                    inner
                        = group
                            .Select(filter.Rewrite)
                            .Aggregate(inner, (current, expression) => current.Or(expression));

                    // Magic here
                    predicate = predicate.And(inner);
                }

                this.Options.Filter = this.Options.Filter != null
                    ? this.Options.Filter.Or(predicate)
                    : PredicateBuilder.False<T>().Or(predicate);
            }

            return this;
        }

        /// <summary>
        /// Expression in which only one <see cref="filter"/> needs to be true for the entire expression to be true. Each
        /// <see cref="TR"/> generates a new <see cref="filter"/>. So 100 terms, will generate 100 expressions, and only 1 needs to be true for the entire expression to be true.
        /// <para>Adds to the expression tree as an 'And' [Current Expression Tree] And [<see cref="filter"/>]</para>
        /// <para>Example: To filter articles by tags, <code>Any([Tag1, Tag2, Tag3], (result, tag) => result.Tags.Contains(tag))</code>,
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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
        /// Expression in which only one <see cref="filter"/> needs to be true for the entire expression to be true. Each
        /// <see cref="TR"/> generates a new <see cref="filter"/>. So 100 terms, will generate 100 expressions, and only 1 needs to be true for the entire expression to be true.
        /// <para>Adds to the expression tree as an 'Or' [Current Expression Tree] Or [<see cref="filter"/>]</para>
        /// <para>Example: To filter articles by tags, <code>Any([Tag1, Tag2, Tag3], (result, tag) => result.Tags.Contains(tag))</code>,
        /// where result is your <see cref="SearchResultItem"/> that contains a property of 'Tags'
        /// </para>
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
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

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="Any{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfAny<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.Any(terms, filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="OrAny{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfOrAny<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.OrAny(terms, filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="Any{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfAll<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.All(terms, filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="Any{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <typeparam name="TR">The type of Term</typeparam>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="terms">Terms that will be passed to the filter expression</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfOrAll<TR>(bool condition, IEnumerable<TR> terms, Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.OrAll(terms, filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="Where"/> expression
        /// to the current expression tree
        /// </summary>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfWhere(bool condition, Expression<Func<T, bool>> filter)
        {
            return condition ? this.Where(filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="OrWhere"/> expression
        /// to the current expression tree
        /// </summary>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public virtual SearchBuilderBase<T> IfOrWhere(bool condition, Expression<Func<T, bool>> filter)
        {
            return condition ? this.OrWhere(filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="ManyAny{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="groups">A grouping of terms</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfManyAny<TR>(bool condition, IEnumerable<IEnumerable<TR>> groups,
            Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.ManyAny(groups, filter) : this;
        }

        /// <summary>
        /// If the <see cref="condition"/> evalutes to <c>True</c> at runtime, will add the <see cref="OrManyAny{TR}"/> expression
        /// to the current expression tree
        /// </summary>
        /// <param name="condition">If <c>True</c> will add to the expression tree at runtime.</param>
        /// <param name="groups">A grouping of terms</param>
        /// <param name="filter">Filter Expression</param>
        /// <returns><see cref="SearchBuilderBase{T}"/></returns>
        public SearchBuilderBase<T> IfOrManyAny<TR>(bool condition, IEnumerable<IEnumerable<TR>> groups,
            Expression<Func<T, TR, bool>> filter)
        {
            return condition ? this.OrManyAny(groups, filter) : this;
        }
    }
}
