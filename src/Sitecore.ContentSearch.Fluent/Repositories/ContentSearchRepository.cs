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
namespace Sitecore.ContentSearch.Fluent.Repositories
{
    using System;
    using System.Linq;
    using Linq;
    using Providers;
    using Results;

    public class ContentSearchRepository : IResultRepository
    {
        private readonly IIndexProvider indexProvider;

        public ContentSearchRepository(IIndexProvider indexProvider)
        {
            this.indexProvider = indexProvider;
        }

        public virtual IQueryable<T> GetQueryable<T>() => this.indexProvider.SearchContext.GetQueryable<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public virtual Linq.SearchResults<T> GetResults<T>(IQueryable<T> queryable) where T : SearchResultItem
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            return queryable.GetResults();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public virtual FacetResults GetFacetResults<T>(IQueryable<T> queryable) where T : SearchResultItem
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            return queryable.GetFacets();
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
                this.indexProvider?.Dispose();
            }
        }

        #endregion
    }
}