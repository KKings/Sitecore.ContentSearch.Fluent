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
