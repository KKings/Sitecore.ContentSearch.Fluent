// <copyright file="FacetOnFilter.cs" company="NavigationArts, LLC">
//  Copyright (c) 2015 NavigationArts, LLC All Rights Reserved
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
        public IList<string> Filters { get; private set; } 

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
