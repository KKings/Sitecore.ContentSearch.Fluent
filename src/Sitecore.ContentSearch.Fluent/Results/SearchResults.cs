// <copyright file="SearchResults.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Results
{
    using System.Collections.Generic;
    using Facets;

    /// <summary>
    /// SearchResults Summary
    /// </summary>
    public class SearchResults<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the Total Results
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the Results
        /// </summary>
        public IList<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the Facets
        /// </summary>
        public IList<FacetValue> Facets { get; set; }
    }
}