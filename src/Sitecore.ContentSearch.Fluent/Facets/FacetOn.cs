// <copyright file="FacetOn.cs" company="NavigationArts, LLC">
//  Copyright (c) 2015 NavigationArts, LLC All Rights Reserved
// </copyright>
namespace Sitecore.ContentSearch.Fluent.Facets
{
    /// <summary>
    /// Default Implementation to implementation
    /// </summary>
    public class FacetOn : FacetBase
    {
        public FacetOn(string key) : base(key) { }

        public FacetOn(string key, int minimumCount) : base(key, minimumCount) { }
    }
}
