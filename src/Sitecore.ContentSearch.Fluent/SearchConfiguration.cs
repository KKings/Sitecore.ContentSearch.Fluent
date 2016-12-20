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
    using Options;
    using Results;

    public class SearchConfiguration<T> where T : SearchResultItem
    {
        /// <summary>
        /// The Paging Options for skipping/taking
        /// </summary>
        public virtual PagingOptions PagingOptions { get; private set; } = new PagingOptions();

        /// <summary>
        /// The Query Filters
        /// </summary>
        public virtual QueryOptions<T> QueryOptions { get; private set; } = new QueryOptions<T>();

        /// <summary>
        /// The Filter Filters =). This is separated out as FilterOptions must be applied to a different method
        /// </summary>
        public virtual FilterOptions<T> FilterOptions { get; private set; } = new FilterOptions<T>();

        /// <summary>
        /// The SortingOptions
        /// </summary>
        public virtual SortingOptions<T> SortingOptions { get; private set; } = new SortingOptions<T>();

        /// <summary>
        /// The FacetOptions
        /// </summary>
        public virtual FacetOptions FacetOptions { get; private set; } = new FacetOptions();
    }
}