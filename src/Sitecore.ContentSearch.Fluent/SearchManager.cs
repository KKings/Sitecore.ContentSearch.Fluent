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
    using Facets;
    using Results;

    /// <summary>
    /// SearchManager manages the lifecycle of the Search Context for the Search Index.
    /// Implements IDisposable to dispose of the SearchContext when the application closes
    /// </summary>
    public class SearchManager : ISearchManager
    {
        public virtual IIndexProvider IndexProvider { get; }

        /// <summary>
        /// Configures the search manager
        /// </summary>
        public SearchManager(IIndexProvider indexProvider)
        {
            this.IndexProvider = indexProvider;
        }

        /// <summary>
        /// Executes a query against the search index
        /// </summary>
        /// <typeparam name="T">Type of ResultItem</typeparam>
        /// <param name="searcherBuilder">Lambda to generate the Search Results Expression</param>
        /// <returns>Result of <see cref="T"/></returns>
        public virtual Results.SearchResults<T> ResultsFor<T>(Action<ISearcher<T>> searcherBuilder)
            where T : SearchResultItem
        {
            var searcher = this.GetSearcher<T>();
            searcherBuilder(searcher);

            return searcher.Results();
        }

        /// <summary>
        /// Gets 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <param name="facets"></param>
        /// <returns></returns>
        public virtual SearchFacets FacetsFor<T>(Action<ISearcher<T>> searcherBuilder, IList<IFacetOn> facets)
            where T : SearchResultItem
        {
            var searcher = this.GetSearcher<T>();
            searcherBuilder(searcher);

            return searcher.Facets(facets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> GetQueryable<T>()
        {
            return this.IndexProvider.SearchContext.GetQueryable<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual ISearcher<T> GetSearcher<T>() where T : SearchResultItem
        {
            return new Searcher<T>(this);
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
                this.IndexProvider?.Dispose();
            }
        }

        #endregion
    }
}