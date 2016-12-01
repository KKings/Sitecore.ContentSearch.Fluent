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
        public string Key { get; }

        /// <summary>
        /// Gets the Minimum Count for the Facet
        /// </summary>
        public int MinimumCount { get; }

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
