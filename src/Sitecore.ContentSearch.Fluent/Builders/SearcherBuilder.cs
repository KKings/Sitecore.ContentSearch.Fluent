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

    public class SearcherBuilder<T> : ISearcherBuilder<T> where T : SearchResultItem
    {
        private readonly SearchConfiguration<T> configuration;

        public SearcherBuilder(SearchConfiguration<T> configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Sets the Paging Options for the Query
        /// </summary>
        /// <param name="searchBuildOptions">Creates a new Instance of PagingOptionsBuilder to build the options
        /// <para>Passes the Query Options as a parameter</para>
        /// </param>
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Paging(Action<PagingOptionsBuilder<T>> searchBuildOptions)
        {
            searchBuildOptions(new PagingOptionsBuilder<T>(this.configuration.PagingOptions));
            return this;
        }

        /// <summary>
        /// Applies filters that will be based by relevancy
        /// </summary>
        /// <param name="searchQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Query(Action<QueryBuilder<T>> searchQueryBuildOptions)
        {
            searchQueryBuildOptions(new QueryBuilder<T>(this.configuration.QueryOptions));
            return this;
        }

        /// <summary>
        /// Sets the SearchOptions
        /// </summary>
        /// <param name="filterQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Filter(Action<FilterBuilder<T>> filterQueryBuildOptions)
        {
            filterQueryBuildOptions(new FilterBuilder<T>(this.configuration.FilterOptions));
            return this;
        }

        /// <summary>
        /// Sets the Sorting Options
        /// </summary>
        /// <param name="sortingBuildOptions">Creates a new Instance of the sortingBuildOptions to build the query
        /// <para>Passes the QueryOptions as a parameter</para></param>      
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Sort(Action<SortingOptionsBuilder<T>> sortingBuildOptions)
        {
            sortingBuildOptions(new SortingOptionsBuilder<T>(this.configuration.SortingOptions));
            return this;
        }

        /// <summary>
        /// Sets the Facet Options
        /// </summary>
        /// <param name="facetBuildOptions">Creates a new instance of <see cref="FacetBuilder{T}"/></param>
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Facet(Action<FacetBuilder<T>> facetBuildOptions)
        {
            facetBuildOptions(new FacetBuilder<T>(this.configuration.FacetOptions));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual ISearcherBuilder<T> Select(Expression<Func<T, T>> expression)
        {
            if (expression != null)
            {
                this.configuration.SelectOptions.Expression = expression;
            }
            
            return this;
        }

        /// <summary>
        /// Sets the within Radius Options
        /// </summary>
        /// <param name="withinRadiusOptions">Creates a new Instance of the WithinRadiusBuilder to build the query
        /// <para>Passes the RadiusOption as a parameter</para></param>      
        /// <returns><see cref="ISearcherBuilder{T}"/></returns>
        public virtual ISearcherBuilder<T> Radius(Action<WithinRadiusBuilder<T>> withinRadiusOptions)
        {
            withinRadiusOptions(new WithinRadiusBuilder<T>(this.configuration.RadiusOptions));
            return this;
        }
    }
}