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
    using System.Collections.Generic;

    public class DefaultIndexProvider : IIndexProvider
    {
        /// <summary>
        /// The Search Context on the Search Index for Searching the Index
        /// </summary>
        private IProviderSearchContext _searchContext;

        /// <summary>
        /// The Search Context on the Search Index for Searching the Index
        /// </summary>
        public virtual IProviderSearchContext SearchContext
        {
            get { return this._searchContext ?? (this._searchContext = this.SearchIndex.CreateSearchContext()); }
        }

        /// <summary>
        /// The Search Index for the Search Manager
        /// </summary>
        private ISearchIndex _searchIndex;

        /// <summary>
        /// The Search Index for the Search Manager
        /// </summary>
        public virtual ISearchIndex SearchIndex
        {
            get { return this._searchIndex ?? (this._searchIndex = ContentSearchManager.GetIndex(this.indexLookup[this.databaseProvider.Context.Name])); }
        }

        /// <summary>
        /// Lookup to match Database Names to Index Names
        /// </summary>
        private readonly IDictionary<string, string> indexLookup;

        /// <summary>
        /// Database Provider Implementation for finding the correct Database
        /// </summary>
        private readonly IDatabaseProvider databaseProvider;

        public DefaultIndexProvider(IDatabaseProvider databaseProvider, IDictionary<string, string> indexLookup)
        {
            this.indexLookup = indexLookup;
            this.databaseProvider = databaseProvider;
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
                this.SearchContext?.Dispose();
            }
        }

        #endregion
    }
}
