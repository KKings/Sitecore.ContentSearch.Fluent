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
    using System.Collections.Generic;
    using System.Linq;
    using Linq;

    /// <summary>
    /// Implementation with Filters
    /// </summary>
    public class FacetOnFilter : FacetBase
    {
        /// <summary>
        /// Gets or sets the Filters
        /// </summary>
        public virtual IList<string> Filters { get; } 

        public FacetOnFilter(string key) : base(key) { }

        public FacetOnFilter(string key, int minimumCount) : base(key, minimumCount) { }

        public FacetOnFilter(string key, int minimumCount, IList<string> filters) : base(key, minimumCount)
        {
            this.Filters = filters ?? new string[0];
        }

        /// <summary>
        /// Adds the Facetable into Queryable. If no Minimum Count is set, it will call the simple constructor
        /// </summary>
        /// <param name="source">Source Queryable</param>
        /// <returns>Queryable with Facet</returns>
        public override IQueryable<T> AddFacet<T>(IQueryable<T> source)
        {
            if (this.Filters.Any())
            {
                return source.FacetOn(f => f[this.Key], this.MinimumCount, this.Filters);
            }

            return base.AddFacet(source);
        }
    }
}
