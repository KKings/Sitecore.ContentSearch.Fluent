// <copyright file="Searcher.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent
{
    using System;
    using System.Linq;
    using Builders;
    using Linq;
    using Linq.Utilities;
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
        /// The QueryOptions
        /// </summary>
        protected readonly FilterOptions<T> FilterOptions;

        /// <summary>
        /// The SortingOptions
        /// </summary>
        protected readonly SortingOptions<T> SortingOptions;

        public Searcher(SearchManager searchManager)
        {
            this.SearcherOptions = new SearcherOptions<T>(searchManager);
            this.QueryOptions = new QueryOptions<T>(this.SearcherOptions.SearchManager.SearchContext.GetQueryable<T>());
            this.FilterOptions = new FilterOptions<T>();
            this.SortingOptions = new SortingOptions<T>();
        }

        /// <summary>
        /// Sets the SearchOptions
        /// </summary>
        /// <param name="searchBuildOptions">Creates a new Instance of SearcherOptionsBuilder to build the options
        /// <para>Passes the Query Options as a parameter</para>
        /// </param>
        /// <returns></returns>
        public Searcher<T> Options(Action<SearcherOptionsBuilder<T>> searchBuildOptions)
        {
            searchBuildOptions(new SearcherOptionsBuilder<T>(this.SearcherOptions));
            return this;
        }

        /// <summary>
        /// Applies filters that will be based by relevancy
        /// </summary>
        /// <param name="searchQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns>Instance of the Searcher</returns>
        public Searcher<T> Query(Action<QueryOptionsBuilder<T>> searchQueryBuildOptions)
        {
            searchQueryBuildOptions(new QueryOptionsBuilder<T>(this.QueryOptions));
            return this;
        }

        /// <summary>
        /// Sets the SearchOptions
        /// </summary>
        /// <param name="filterQueryBuildOptions">Creates a new Instance of the QueryOptionsBuilder to build the query
        /// <para>Passes the Searcher Options as a parameter</para>
        /// </param>
        /// <returns>Instance of the Searcher</returns>
        public Searcher<T> Filter(Action<FilterOptionsBuilder<T>> filterQueryBuildOptions)
        {
            filterQueryBuildOptions(new FilterOptionsBuilder<T>(this.FilterOptions));
            return this;
        }

        /// <summary>
        /// Sets the Sorting Options
        /// </summary>
        /// <param name="sortingBuildOptions">Creates a new Instance of the sortingBuildOptions to build the query
        /// <para>Passes the QueryOptions as a parameter</para></param>      
        /// <returns>Instance of the Searcher</returns>
        public Searcher<T> Sort(Action<SortingOptionsBuilder<T>> sortingBuildOptions)
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
        public Results.SearchResults<T> Results()
        {
            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Where(this.QueryOptions.Filter);

            // Setup a predicate builder as an easy way to build up predicate
            var filter = PredicateBuilder.True<T>();

            if (this.SearcherOptions.Restrictions.Any())
            {
                // Restrict search to limited number of templates (only resource items) using an Or on the predicate
                filter = this.SearcherOptions.Restrictions.Aggregate(filter, (current, t) => current.Or(p => p.TemplateId == t));
            }

            filter = filter.And(this.FilterOptions.Filter);

            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Filter(filter);

            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Skip(this.SearcherOptions.StartingPosition);

            if (this.SearcherOptions.Display > 0)
            {
                this.QueryOptions.Queryable = this.QueryOptions.Queryable.Take(this.SearcherOptions.Display);
            }

            if (this.SortingOptions.Expressions.Any())
            {
                this.QueryOptions.Queryable = this.SortingOptions.ApplySorting(this.QueryOptions.Queryable);
            }

            var results = this.QueryOptions.Queryable.GetResults();

            // Removed the Sitecore Mapping and only used fields stored in the index
            return new Results.SearchResults<T>
            {
                Total = results.TotalSearchResults,
                Results = results.Hits.Select(x => x.Document).ToList()
            };
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
        public SearchFacets Facets(string[] facets)
        {
            // Setup a predicate builder as an easy way to build up predicate
            var filter = PredicateBuilder.True<T>();

            if (this.SearcherOptions.Restrictions.Any())
            {
                // Restrict search to limited number of templates (only resource items) using an Or on the predicate
                filter = this.SearcherOptions.Restrictions.Aggregate(filter, (current, t) => current.Or(p => p.TemplateId == t));
            }

            filter = filter.And(this.FilterOptions.Filter);

            this.QueryOptions.Queryable = this.QueryOptions.Queryable.Filter(filter);

            if (facets.Any())
            {
                this.QueryOptions.Queryable = facets.Aggregate(this.QueryOptions.Queryable, (current, facetName) => current.FacetOn(c => c[facetName]));
            }

            var results = this.QueryOptions.Queryable.GetFacets();

            return new SearchFacets
            {
                Total = results.Categories.Count(),
                Facets = results.Categories.Select(facet => new Facets.FacetCategory
                {
                    Name = facet.Name,
                    Values = facet.Values
                                  .Select(x => new FacetValue(x.Name, x.AggregateCount))
                                  .Where(x => x != null || x.Name != null)
                                  .ToArray()
                }).ToArray()
            };
        }
    }
}