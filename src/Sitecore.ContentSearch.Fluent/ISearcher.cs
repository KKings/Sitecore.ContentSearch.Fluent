// <copyright file="ISearcher.cs" company="Kyle Kingsbury">
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
    using System.Linq.Expressions;
    using Builders;
    using Facets;
    using Linq;
    using Results;

    public interface ISearcher<T> where T : SearchResultItem
    {
        Results.SearchResults<T> Results();
        SearchFacets Facets(IList<IFacetOn> facets);
        Searcher<T> Paging(Action<PagingOptionsBuilder<T>> searchBuildOptions);
        Searcher<T> Query(Action<QueryBuilder<T>> searchQueryBuildOptions);
        Searcher<T> Filter(Action<FilterBuilder<T>> filterQueryBuildOptions);
        Searcher<T> Sort(Action<SortingOptionsBuilder<T>> sortingBuildOptions);
        IQueryable<T> Filter(IQueryable<T> queryable, Expression<Func<T, bool>> predicate);
        Linq.SearchResults<T> GetResults(IQueryable<T> queryable);
        FacetResults GetFacets(IQueryable<T> queryable, IList<IFacetOn> facets);
    }
}
