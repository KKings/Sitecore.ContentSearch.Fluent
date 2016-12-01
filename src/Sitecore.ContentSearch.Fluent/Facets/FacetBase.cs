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
namespace Sitecore.ContentSearch.Fluent.Facets
{
    using System;
    using System.Linq;
    using Linq;
    using SearchResultItem = Results.SearchResultItem;

    /// <summary>
    /// Default Implementation of a Facet
    /// </summary>
    public abstract class FacetBase : IFacetOn
    {
        /// <summary>
        /// Gets the Key to Facet On
        /// </summary>
        public virtual string Key { get; }

        /// <summary>
        /// Gets the Minimum Count for the Facet
        /// </summary>
        public virtual int MinimumCount { get; }

        protected FacetBase(string key) : this(key, Int32.MinValue) { }

        protected FacetBase(string key, int minimumCount)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.Key = key;
            this.MinimumCount = minimumCount;
        }

        /// <summary>
        /// Adds the Facetable into Queryable. If no Minimum Count is set, it will call the simple constructor
        /// </summary>
        /// <param name="source">Source Queryable</param>
        /// <returns>Queryable with Facet</returns>
        public virtual IQueryable<T> AddFacet<T>(IQueryable<T> source) where T : SearchResultItem
        {
            return this.MinimumCount != Int32.MinValue
                ? source.FacetOn(f => f[this.Key], this.MinimumCount)
                : source.FacetOn(f => f[this.Key]);
        }
    }
}
