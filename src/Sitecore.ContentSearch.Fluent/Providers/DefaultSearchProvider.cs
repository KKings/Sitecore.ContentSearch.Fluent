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
namespace Sitecore.ContentSearch.Fluent.Providers
{
    using System;
    using System.Linq;
    using Builders;
    using Linq;
    using Repositories;
    using Results;
    using Services;
    using Sitecore.Abstractions;
    using FacetValue = Facets.FacetValue;

    public class DefaultSearchProvider : ISearchProvider
    {
        /// <summary>
        /// Implementation of the Index Provider
        /// </summary>
        private readonly IResultRepository resultRepository;

        /// <summary>
        /// Implementation of the Query Service
        /// </summary>
        private readonly IQueryService queryService;

        /// <summary>
        /// Implementation of the Logger
        /// </summary>
        private readonly BaseLog logger;

        public DefaultSearchProvider(IResultRepository resultRepository, IQueryService queryService, BaseLog logger)
        {
            this.resultRepository = resultRepository;
            this.queryService = queryService;
            this.logger = logger;
        }

        public virtual Results.SearchResults<T> GetResults<T>(SearchConfiguration<T> configuration) where T : SearchResultItem
        {
            var queryable = this.resultRepository.GetQueryable<T>();

            queryable = this.queryService.ApplyQuery(queryable, configuration.QueryOptions);
            queryable = this.queryService.ApplyFilter(queryable, configuration.FilterOptions);
            queryable = this.queryService.ApplyPagination(queryable, configuration.PagingOptions);
            queryable = this.queryService.ApplySorting(queryable, configuration.SortingOptions);
            queryable = this.queryService.ApplyProjection(queryable, configuration.SelectOptions);
            queryable = this.queryService.ApplyWithinRadius(queryable, configuration.RadiusOptions);

            var rawResults = this.resultRepository.GetResults(queryable);

            if (rawResults != null)
            {
                return this.ProcessResults(rawResults);
            }

            this.logger.Error(
                "An unknown error occurred. This is likely a communication error with your search provider, " +
                "please check the logs and ensure the solution is able to connect to the indexes properly.", 
                this);

            rawResults = new Linq.SearchResults<T>(Enumerable.Empty<SearchHit<T>>(), 0);

            return this.ProcessResults(rawResults);
        }

        public virtual SearchFacetResults GetFacetResults<T>(SearchConfiguration<T> configuration) where T : SearchResultItem
        {
            var queryable = this.resultRepository.GetQueryable<T>();

            queryable = this.queryService.ApplyQuery(queryable, configuration.QueryOptions);
            queryable = this.queryService.ApplyFilter(queryable, configuration.FilterOptions);
            queryable = this.queryService.ApplyFacets(queryable, configuration.FacetOptions);

            var rawResults = this.resultRepository.GetFacetResults(queryable);

            return this.ProcessFacetResults(rawResults);
        }

        public virtual Results.SearchResults<T> ProcessResults<T>(Linq.SearchResults<T> results) where T : SearchResultItem
        {
            var tempResults = results ?? new Linq.SearchResults<T>(Enumerable.Empty<SearchHit<T>>(), 0);

            return new Results.SearchResults<T>(
                tempResults.Hits,
                tempResults.TotalSearchResults);
        }

        public virtual ISearcherBuilder<T> GetSearcherBuilder<T>(SearchConfiguration<T> configuration) where T : SearchResultItem
        {
            return new SearcherBuilder<T>(configuration);
        }

        public virtual SearchFacetResults ProcessFacetResults(FacetResults results)
        {
            return new SearchFacetResults
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

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.resultRepository?.Dispose();
            }
        }

        #endregion
    }
}