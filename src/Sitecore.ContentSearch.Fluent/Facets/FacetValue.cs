﻿// <copyright file="FacetValue.cs" company="Kyle Kingsbury">
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
    /// <summary>
    /// FacetValue Summary
    /// </summary>
    public class FacetValue
    {
        /// <summary>
        /// Gets or sets the Facet Name 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the Count or aggregate value
        /// </summary>
        public int Count { get; private set; }

        public FacetValue(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
    }
}