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
namespace Sitecore.ContentSearch.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Builders;
    using Facets;
    using Linq;
    using Options;
    using Results;
    using FacetValue = Facets.FacetValue;

    /// <summary>
    /// Searcher Summary
    /// </summary>
    public class Searcher<T> : ISearcher<T> where T : SearchResultItem
    {
        /// <summary>
        /// The SearcherOptions 
        /// </summary>
        protected readonly SearcherOptions<T> SearcherOptions;

        /// <summary>
        /// The QueryOptions
        /// </summary>
        protected readonly QueryOptions<T> QueryOptions;

        /// <summary>
        /// The FilterOptions. This is separated out as FilterOptions must be applied to a different method
        /// </summary>
        protected readonly FilterOptions<T> FilterOptions;

        /// <summary>
        /// The SortingOptions
        /// </summary>
        protected readonly SortingOptions<T> SortingOptions;

        public Searcher(ISearchManager searchManager)
        {
            this.SearcherOptions = new SearcherOptions<T>(searchManager);
            this.QueryOptions = new QueryOptions<T>(this.SearcherOptions.SearchManager.GetQueryable<T>());
            this.FilterOptions = new FilterOptions<T>();
            this.SortingOptions = new SortingOptions<T>();
        }

        /// <summary>
        /// Sets the Paging Options for the Query
        /// </summary>
        /// <param name="searchBuildOptions">Creates a new Instance of PagingOptionsBuilder to build the options
        /// <para>Passes the Query Options as a parameter</para>
        /// </param>
        /// <returns></returns>
        public virtual Searcher<T> Paging(Action<PagingOptionsBuilder<T>> searchBuildOptions)
        {
            searchBuildOptions(new PagingOptionsBuilder<T>(this.SearcherOptions));
            return this;
        }

        /// <summary>
        /// Applies filters that will be based by relevancy
        /// </summary>
        /// <param name="searchQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns>Instance of the Searcher</returns>
        public virtual Searcher<T> Query(Action<QueryBuilder<T>> searchQueryBuildOptions)
        {
            searchQueryBuildOptions(new QueryBuilder<T>(this.QueryOptions));
            return this;
        }

        /// <summary>
        /// Sets the SearchOptions
        /// </summary>
        /// <param name="filterQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns>Instance of the Searcher</returns>
        public virtual Searcher<T> Filter(Action<FilterBuilder<T>> filterQueryBuildOptions)
        {
            filterQueryBuildOptions(new FilterBuilder<T>(this.FilterOptions));
            return this;
        }

        /// <summary>
        /// Sets the Sorting Options
        /// </summary>
        /// <param name="sortingBuildOptions">Creates a new Instance of the sortingBuildOptions to build the query
        /// <para>Passes the QueryOptions as a parameter</para></param>      
        /// <returns>Instance of the Searcher</returns>
        public virtual Searcher<T> Sort(Action<SortingOptionsBuilder<T>> sortingBuildOptions)
        {
            sortingBuildOptions(new SortingOptionsBuilder<T>(this.SortingOptions));
            return this;
        }

        /// <summary>
        /// Fetches the results from the Index provided in the SearchManager.
        /// <para>Restrictions are filtered out through the options</para>
        /// <para>Sorting is done through the options</para>
        /// <para>For all options, see <see cref="SearcherOptions"/>SearcherOptions</para>
        /// <para>For all query options, see <see cref="QueryOptions"/>QueryOptions</para>
        /// </summary>
        /// <returns>Search Results of T</returns>
        public virtual Results.SearchResults<T> Results()
        {
            if (this.QueryOptions.Filter != null)
            {
                this.QueryOptions.Queryable = this.QueryOptions.Queryable.Where(this.QueryOptions.Filter);
            }

            if (this.FilterOptions.Filter != null)
            {
                this.QueryOptions.Queryable = this.Filter(this.QueryOptions.Queryable, this.FilterOptions.Filter);
            }

            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Skip(this.SearcherOptions.StartingPosition);

            if (this.SearcherOptions.Display > 0)
            {
                this.QueryOptions.Queryable = this.QueryOptions.Queryable.Take(this.SearcherOptions.Display);
            }

            if (this.SortingOptions.Expressions.Any())
            {
                this.QueryOptions.Queryable = this.SortingOptions.ApplySorting(this.QueryOptions.Queryable);
            }

            var results = this.GetResults(this.QueryOptions.Queryable);

            // Removed the Sitecore Mapping and only used fields stored in the index
            return new Results.SearchResults<T>(
                results: results.Hits,
                total: results.TotalSearchResults);
        }

        /// <summary>
        /// Fetches the facets from the Index provided in the SearchManager.
        /// <para>Restrictions are filtered out through the options</para>
        /// <para>Sorting is done through the options</para>
        /// <para>For all options, see <see cref="SearcherOptions"/>SearcherOptions</para>
        /// <para>For all query options, see <see cref="QueryOptions"/>QueryOptions</para>
        /// </summary>
        /// <param name="facets">Array of strings to facet on</param>
        /// <returns>Facets for Query</returns>
        public virtual SearchFacets Facets(IList<IFacetOn> facets)
        {
            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Where(this.QueryOptions.Filter);

            if (this.FilterOptions.Filter != null)
            {
                this.QueryOptions.Queryable = this.Filter(this.QueryOptions.Queryable, this.FilterOptions.Filter);
            }

            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Skip(this.SearcherOptions.StartingPosition);

            var results = this.GetFacets(this.QueryOptions.Queryable, facets);

            return new SearchFacets
            {
                Total = results.Categories.Count,
                Facets = results.Categories.Select(facet =>
                    new Facets.FacetCategory(facet.Name,
                        facet.Values
                             .Select(x => new FacetValue(x.Name, x.AggregateCount))
                             .Where(x => x?.Name != null)
                             .ToArray())
                    ).ToArray()
            };
        }

        /// <summary>
        /// Apples a Filter to the Queryable
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="predicate">Predicate to Apply</param>
        public virtual IQueryable<T> Filter(IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            return queryable.Filter(predicate);
        }
        
        /// <summary>
        /// Gets the SearchResults from a Queryable
        /// </summary>
        /// <returns>The Search Results</returns>
        public virtual Linq.SearchResults<T> GetResults(IQueryable<T> queryable)
        {
            return queryable.GetResults();
        }

        /// <summary>
        /// Gets the FacetResults from the Queryable
        /// </summary>
        /// <param name="queryable">The Search Facets</param>
        /// <param name="facets">Facets</param>
        /// <returns>The Facet Results</returns>
        public virtual FacetResults GetFacets(IQueryable<T> queryable, IList<IFacetOn> facets)
        {
            if (facets.Any())
            {
                queryable = facets.Aggregate(queryable, (current, facet) => facet.AddFacet(current));
            }

            return queryable.GetFacets();
        }
    }
}