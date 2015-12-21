// <copyright file="IFacetOn.cs" company="NavigationArts, LLC">
//  Copyright (c) 2015 NavigationArts, LLC All Rights Reserved
// </copyright>
namespace Sitecore.ContentSearch.Fluent.Facets
{
    using System.Linq;
    using SearchResultItem = Results.SearchResultItem;

    /// <summary>
    /// Contract for Implementing Facets
    /// </summary>
    public interface IFacetOn
    {
        /// <summary>
        /// Add Facet into the Queryable
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AddFacet<T>(IQueryable<T> source) where T : SearchResultItem;
    }
}
