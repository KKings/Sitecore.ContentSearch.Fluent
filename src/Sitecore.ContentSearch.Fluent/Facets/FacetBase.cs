// <copyright file="FacetBase.cs" company="NavigationArts, LLC">
//  Copyright (c) 2015 NavigationArts, LLC All Rights Reserved
// </copyright>
namespace Sitecore.ContentSearch.Fluent.Facets
{
    using System;
    using System.Linq;
    using Linq;
    using SearchResultItem = Results.SearchResultItem;

    /// <summary>
    /// Default 
    /// </summary>
    public abstract class FacetBase : IFacetOn
    {
        /// <summary>
        /// Gets the Key to Facet On
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the Minimum Count for the Facet
        /// </summary>
        public int MinimumCount { get; private set; }

        protected FacetBase(string key) : this(key, Int32.MinValue) { }

        protected FacetBase(string key, int minimumCount)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty");
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
            return (this.MinimumCount != Int32.MinValue)
                ? source.FacetOn(f => f[this.Key], this.MinimumCount)
                : source.FacetOn(f => f[this.Key]);
        }
    }
}
