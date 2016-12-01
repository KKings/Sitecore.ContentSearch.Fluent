// <copyright file="FacetCategory.cs" company="Kyle Kingsbury">
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
        public IList<string> Filters { get; } 

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
