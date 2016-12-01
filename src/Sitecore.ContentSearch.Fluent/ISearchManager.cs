// <copyright file="ISearchManager.cs" company="Kyle Kingsbury">
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
    using Linq;
    using Results;

    public interface ISearchManager : IDisposable
    {
        /// <summary>
        /// Results for a Search Builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        Results.SearchResults<T> ResultsFor<T>(Action<ISearcher<T>> searcherBuilder) where T : SearchResultItem;

        /// <summary>
        /// Facet Results for a SearchBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <param name="facets"></param>
        /// <returns></returns>
        SearchFacets FacetsFor<T>(Action<ISearcher<T>> searcherBuilder, IList<IFacetOn> facets) where T : SearchResultItem;

        /// <summary>
        /// Get the Queryable
        /// </summary>
        /// <typeparam name="T" />
        /// <returns></returns>
        IQueryable<T> GetQueryable<T>();

        /// <summary>
        /// Get the Searcher
        /// </summary>
        ISearcher<T> GetSearcher<T>() where T: SearchResultItem;
    }
}
