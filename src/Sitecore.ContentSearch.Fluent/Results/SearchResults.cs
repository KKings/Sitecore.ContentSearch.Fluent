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
namespace Sitecore.ContentSearch.Fluent.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using Linq;
    using FacetValue = Facets.FacetValue;

    /// <summary>
    /// SearchResults Summary
    /// </summary>
    public class SearchResults<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the Total Results
        /// </summary>
        public virtual int Total { get; private set; }

        /// <summary>
        /// Gets or sets the Results
        /// </summary>
        public virtual IEnumerable<SearchHit<T>> Hits { get; }

        /// <summary>
        /// Gets or sets the Facets
        /// </summary>
        public virtual IList<FacetValue> Facets { get; private set; }

        /// <summary>
        /// Gets only the Documents within the Search Results
        /// </summary>
        public virtual IList<T> Results
        {
            get { return this.Hits.Select(m => m.Document).ToArray(); }
        }

        public SearchResults(IEnumerable<SearchHit<T>> results, int total, IList<FacetValue> facets = null)
        {
            this.Hits = results;
            this.Total = total;
            this.Facets = facets ?? new FacetValue[0];
        }
    }
}