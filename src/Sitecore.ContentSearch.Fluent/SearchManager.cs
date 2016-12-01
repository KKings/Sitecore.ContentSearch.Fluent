// <copyright file="SearchManager.cs" company="Kyle Kingsbury">
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