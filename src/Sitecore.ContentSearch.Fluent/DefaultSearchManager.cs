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
            var results = this.SearchProvider.GetResults(configuration);

            return results;
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
            var results = this.SearchProvider.GetFacetResults(configuration);

            return results;
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
}