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
namespace Sitecore.ContentSearch.Fluent.Services
{
    using System.Linq;
    using Options;
    using Results;

    public interface IQueryService
    {
        /// <summary>
        /// Apples a Filter to the Queryable
        /// </summary>
        /// <param name="queryable">The Queryable</param>
        /// <param name="filterOptions">The FilterOptions</param>
        IQueryable<T> ApplyFilter<T>(IQueryable<T> queryable, FilterOptions<T> filterOptions) where T : SearchResultItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="facetOptions"></param>
        /// <returns></returns>
        IQueryable<T> ApplyFacets<T>(IQueryable<T> queryable, FacetOptions facetOptions) where T : SearchResultItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IQueryable<T> ApplyQuery<T>(IQueryable<T> queryable, QueryOptions<T> options) where T : SearchResultItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IQueryable<T> ApplyPagination<T>(IQueryable<T> queryable, PagingOptions options) where T : SearchResultItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, SortingOptions<T> options) where T : SearchResultItem;
    }
}