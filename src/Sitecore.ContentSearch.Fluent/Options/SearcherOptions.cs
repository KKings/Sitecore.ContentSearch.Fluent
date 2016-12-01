// <copyright file="SearcherOptions.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Linq.Utilities;
    using Data;
    using Results;

    /// <summary>
    /// SearcherOptions Summary
    /// </summary>
    public sealed class SearcherOptions<T> where T : SearchResultItem
    {
        /// <summary>
        ///  Gets or sets the Search Manager
        /// </summary>
        public ISearchManager SearchManager { get; set; }

        /// <summary>
        /// Gets or sets the Template Restrictions
        /// </summary>
        public IList<ID> Restrictions { get; set; }

        /// <summary>
        /// Gets or sets the Facets to aggregate on
        /// </summary>
        public IList<string> Facets { get; set; }

        /// <summary>
        /// Gets or sets the Sorting order
        /// </summary>
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the PageMode
        /// </summary>
        public PageMode PageMode { get; set; }

        /// <summary>
        /// Gets or sets the Start
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the returned results
        /// </summary>
        public int Display { get; set; }

        /// <summary>
        /// Gets the calculating StartingPosition
        /// </summary>
        public int StartingPosition
        {
            get
            {
                // If the PageMode is the Pager, we need to calculate the starting position
                if (this.PageMode == PageMode.Pager)
                {
                    return this.Start <= 1
                            ? 0
                            : (this.Start - 1) * this.Display;
                }
                    
                return this.Start;
            }
        }

        /// <summary>
        /// Gets or sets the Filter expressions
        /// </summary>
        public Expression<Func<T, bool>> Filter { get; set; }

        /// <summary>
        /// Gets or sets the Property to Sort By
        /// </summary>
        public Expression<Func<T, object>> SortBy { get; set; }

        public SearcherOptions(ISearchManager searchManager)
        {
            if (searchManager == null)
            {
                throw new ArgumentNullException(nameof(searchManager));
            }

            this.Restrictions = new List<ID>();
            this.Facets = new List<string>();
            this.SortOrder = SortOrder.Ascending;
            this.SearchManager = searchManager;
            this.Filter = PredicateBuilder.True<T>();
            this.PageMode = PageMode.Pager;
        }
    }
}