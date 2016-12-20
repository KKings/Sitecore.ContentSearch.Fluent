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
    using Linq;
    using Options;
    using Results;

    public class QueryService : IQueryService
    {
        /// <summary>
        /// Apples a Filter to the Queryable
        /// </summary>
        /// <param name="queryable">The Queryable</param>
        /// <param name="filterOptions">The FilterOptions</param>
        public virtual IQueryable<T> ApplyFilter<T>(IQueryable<T> queryable, FilterOptions<T> filterOptions)
            where T : SearchResultItem
        {
            if (filterOptions?.Filter != null)
            {
                queryable = queryable.Filter(filterOptions.Filter);
            }

            return queryable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="facetOptions"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplyFacets<T>(IQueryable<T> queryable, FacetOptions facetOptions) where T : SearchResultItem
        {
            if (facetOptions != null && facetOptions.Facets.Any())
            {
                queryable = facetOptions.Facets.Aggregate(queryable, (current, facet) => facet.AddFacet(current));
            }

            return queryable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplyQuery<T>(IQueryable<T> queryable, QueryOptions<T> options) where T : SearchResultItem
        {
            if (options?.Filter != null)
            {
                queryable = queryable.Where(options.Filter);
            }

            return queryable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplyPagination<T>(IQueryable<T> queryable, PagingOptions options) where T : SearchResultItem
        {
            if (options.StartingPosition > 0)
            {
                queryable = queryable.Skip(options.StartingPosition);
            }

            if (options.Display > 0)
            {
                queryable = queryable.Take(options.Display);
            }

            return queryable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplySorting<T>(IQueryable<T> queryable, SortingOptions<T> options) where T : SearchResultItem
        {
            if (options == null || !options.Expressions.Any())
            {
                return queryable;
            }

            // Resolve bug with Sitecore not evaluating orders correctly
            // http://www.daveleigh.co.uk/sitecore-content-search-api-thenby-clause-not-evaluating-correctly/
            var expressions = options.Expressions.Reverse();

            var operations = expressions as SortingOptions<T>.SortingOperation[] ?? expressions.ToArray();

            var orderByExpression = operations.First();

            var orderedQueryable = orderByExpression.SortOrder == SortOrder.Ascending
                ? queryable.OrderBy(orderByExpression.Expression)
                : queryable.OrderByDescending(orderByExpression.Expression);

            return operations.Skip(1)
                             .Aggregate(orderedQueryable,
                                 (current, expression) =>
                                     expression.SortOrder == SortOrder.Ascending
                                         ? current.ThenBy(expression.Expression)
                                         : current.ThenByDescending(expression.Expression));
        }
    }
}