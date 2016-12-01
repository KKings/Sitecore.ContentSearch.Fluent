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

    /// <summary>
    /// Represents a Facet by Name
    /// </summary>
    public class FacetCategory
    {
        /// <summary>
        /// Gets or sets the Name of the Facet
        /// <para>Note, this is the name within the index</para>
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Gets of sets the Facet Values
        /// </summary>
        public virtual IList<FacetValue> Values { get; }

        public FacetCategory(string name, IList<FacetValue> values)
        {
            this.Name = name;
            this.Values = values ?? new FacetValue[0];
        }
    }
}
