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
    using Builders;
    using Providers;
    using Results;

    /// <summary>
    /// SearchManager manages the lifecycle of the Search Context for the Search Index.
    /// Implements IDisposable to dispose of the SearchContext when the application closes
    /// </summary>
    public class DefaultSearchManager : ISearchManager
    {
        /// <summary>
        /// Gets the Search Provider Implementation
        /// </summary>
        internal virtual ISearchProvider SearchProvider { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="searchProvider">The Search Provider</param>
        public DefaultSearchManager(ISearchProvider searchProvider)
        {
            this.SearchProvider = searchProvider;
        }

        /// <summary>
        /// Executes a query against the search index
        /// </summary>
        /// <typeparam name="T">Type of ResultItem</typeparam>
        /// <param name="searcherBuilder">Lambda to generate the Search Results Expression</param>
        /// <returns>Result of <see cref="T"/></returns>
        public virtual SearchResults<T> ResultsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder)
            where T : SearchResultItem
        {
            var configuration = new SearchConfiguration<T>();
            var searcher = this.SearchProvider.GetSearcherBuilder(configuration);

            // Build Options
            searcherBuilder(searcher);

            // Get Results and Process
            return this.ResultsFor(configuration);
        }

        /// <summary>
        /// Gets the Search Facets for a given Search
        /// </summary>
        /// <param name="searcherBuilder">Configurable Search Builder</param>
        /// <returns></returns>
        public virtual SearchFacetResults FacetsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder)
            where T : SearchResultItem
        {
            var configuration = new SearchConfiguration<T>();
            var searcher = this.SearchProvider.GetSearcherBuilder(configuration);

            // Build the options
            searcherBuilder(searcher);

            // Get Results and Process
            return this.FacetsFor(configuration);
        }

        /// <summary>
        /// Gets the Search Results and Facets for a given Search
        /// </summary>
        /// <param name="searcherBuilder">Configurable Search Builder</param>
        /// <returns></returns>
        public virtual SearchResultsWithFacets<T> ResultsWithFacetsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder)
            where T : SearchResultItem
        {
            var configuration = new SearchConfiguration<T>();
            var searcher = this.SearchProvider.GetSearcherBuilder(configuration);

            // Build the options
            searcherBuilder(searcher);

            throw new NotImplementedException();

            //return searcher.ResultsWithFacets(facets);
        }

        /// <summary>
        /// Gets the results for a given configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public virtual SearchResults<T> ResultsFor<T>(SearchConfiguration<T> configuration)
            where T : SearchResultItem
        {
            // Get Results and Process
            // Todo: Validate
            return this.SearchProvider.GetResults(configuration);
        }

        /// <summary>
        /// Gets the IQueryable before it is executed
        /// </summary>
        public virtual IQueryable<T> GetQueryable<T>(Action<ISearcherBuilder<T>> searcherBuilder) where T : SearchResultItem
        {
            var configuration = new SearchConfiguration<T>();
            var searcher = this.SearchProvider.GetSearcherBuilder(configuration);

            // Build Options
            searcherBuilder(searcher);

            return this.SearchProvider.GetQueryable(configuration);
        }

        /// <summary>
        /// Gets the results for a given configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public virtual SearchFacetResults FacetsFor<T>(SearchConfiguration<T> configuration)
            where T : SearchResultItem
        {
            // Get Results and Process
            // Todo: Validate
            return this.SearchProvider.GetFacetResults(configuration);
        }

        /// <summary>
        /// Gets the built configuration to pass around
        /// </summary>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        public virtual SearchConfiguration<T> Build<T>(Action<ISearcherBuilder<T>> searcherBuilder)
            where T : SearchResultItem
        {
            var configuration = new SearchConfiguration<T>();
            var searcher = this.SearchProvider.GetSearcherBuilder(configuration);

            // Build the options
            searcherBuilder(searcher);

            return configuration;
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
                this.SearchProvider?.Dispose();
            }
        }

        #endregion
    }

    public class DefaultSearchManager<T> : ISearchManager<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets the Search Provider Implementation
        /// </summary>
        internal virtual ISearchManager SearchManager { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="searchManager">The Search Provider</param>
        public DefaultSearchManager(ISearchManager searchManager)
        {
            this.SearchManager = searchManager;
        }
        
        /// <summary>
        /// Gets the IQueryable before it is executed
        /// </summary>
        public virtual IQueryable<T> GetQueryable(Action<ISearcherBuilder<T>> searcherBuilder)
        {
            return this.SearchManager.GetQueryable(searcherBuilder);
        }

        /// <summary>
        /// Executes a query against the search index
        /// </summary>
        /// <typeparam name="T">Type of ResultItem</typeparam>
        /// <param name="searcherBuilder">Lambda to generate the Search Results Expression</param>
        /// <returns>Result of <see cref="T"/></returns>
        public virtual SearchResults<T> ResultsFor(Action<ISearcherBuilder<T>> searcherBuilder)
        {
            return this.SearchManager.ResultsFor(searcherBuilder);
        }

        /// <summary>
        /// Gets the Search Facets for a given Search
        /// </summary>
        /// <param name="searcherBuilder">Configurable Search Builder</param>
        /// <returns></returns>
        public virtual SearchFacetResults FacetsFor(Action<ISearcherBuilder<T>> searcherBuilder)
        {
            return this.SearchManager.FacetsFor(searcherBuilder);
        }

        /// <summary>
        /// Gets the Search Results and Facets for a given Search
        /// </summary>
        /// <param name="searcherBuilder">Configurable Search Builder</param>
        /// <returns></returns>
        public virtual SearchResultsWithFacets<T> ResultsWithFacetsFor(Action<ISearcherBuilder<T>> searcherBuilder)
        {
            return this.SearchManager.ResultsWithFacetsFor(searcherBuilder);
        }

        /// <summary>
        /// Gets the results for a given configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public virtual SearchResults<T> ResultsFor(SearchConfiguration<T> configuration)
        {
            return this.SearchManager.ResultsFor(configuration);
        }

        /// <summary>
        /// Gets the results for a given configuration
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public virtual SearchFacetResults FacetsFor(SearchConfiguration<T> configuration)
        {
            return this.SearchManager.FacetsFor(configuration);
        }

        /// <summary>
        /// Gets the built configuration to pass around
        /// </summary>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        public virtual SearchConfiguration<T> Build(Action<ISearcherBuilder<T>> searcherBuilder)
        {
            return this.SearchManager.Build(searcherBuilder);
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
                this.SearchManager?.Dispose();
            }
        }

        #endregion
    }
}