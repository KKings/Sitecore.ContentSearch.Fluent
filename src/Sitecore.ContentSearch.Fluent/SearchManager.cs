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
    using Sitecore;
    using ContentSearch;
    using Linq;
    using Data;

    /// <summary>
    /// SearchManager manages the lifecycle of the Search Context for the Search Index.
    /// Implements IDisposable to dispose of the SearchContext when the application closes
    /// </summary>
    public class SearchManager : ISearchManager
    {
        /// <summary>
        /// The Database, default to Web if not in Context.Database is null
        /// </summary>
        private Database Database
        {
            get { return Context.Database ?? Configuration.Factory.GetDatabase("web"); }
        } 
        /// <summary>
        /// The Search Index for the Search Manager
        /// </summary>
        private ISearchIndex _searchIndex;

        /// <summary>
        /// The Search Index for the Search Manager
        /// </summary>
        public ISearchIndex SearchIndex
        {
            get
            {
                return _searchIndex ??
                       (_searchIndex =
                           ContentSearchManager.GetIndex(this.Database.Name.Equals("master", StringComparison.InvariantCultureIgnoreCase)
                               ? this._masterIndexName
                               : this._webIndexName));
            }
        }

        /// <summary>
        /// The Search Context on the Search Index for Searching the Index
        /// </summary>
        private IProviderSearchContext _searchContext;

        /// <summary>
        /// The Search Context on the Search Index for Searching the Index
        /// </summary>
        public IProviderSearchContext SearchContext
        {
            get { return this._searchContext ?? (this._searchContext = this.SearchIndex.CreateSearchContext()); }
        }

        /// <summary>
        /// Index name when using the Web database
        /// </summary>
        private readonly string _webIndexName;

        /// <summary>
        /// Index name when using the Master database
        /// </summary>
        private readonly string _masterIndexName;

        /// <summary>
        /// Configures the Search Manager
        /// </summary>
        /// <param name="webIndexName">Index name when using the Web database</param>
        /// <param name="masterIndexName">Index name when using the Master database</param>
        public SearchManager(string webIndexName, string masterIndexName)
        {
            this._webIndexName = webIndexName;
            this._masterIndexName = masterIndexName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        public Results.SearchResults<T> ResultsFor<T>(Action<Searcher<T>> searcherBuilder)
            where T : Results.SearchResultItem
        {
            var searcher = new Searcher<T>(this);
            searcherBuilder(searcher);

            return searcher.Results();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <param name="fieldFacets"></param>
        /// <returns></returns>
        public Results.SearchFacets FacetsFor<T>(Action<Searcher<T>> searcherBuilder, string[] fieldFacets)
            where T : Results.SearchResultItem
        {
            var searcher = new Searcher<T>(this);
            searcherBuilder(searcher);

            return searcher.Facets(fieldFacets);
        }

        /// <summary>
        /// Maps additional fields not stored within the Index to Sitecore Items
        /// </summary>
        /// <param name="results">Search Results from the Index</param>
        /// <returns>List of TModels updated with additional Fields</returns>
        public IList<TModel> Map<TModel>(IEnumerable<SearchHit<TModel>> results)
        {
            throw new NotImplementedException();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing && this.SearchContext != null)
            {
                this.SearchContext.Dispose();
            }
        }

        #endregion
    }
}