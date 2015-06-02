// <copyright file="SearcherOptionsBuilder.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Builders
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Utilities;
    using Data;
    using Sitecore.Diagnostics;
    using Options;
    using Results;

    /// <summary>
    /// SearcherOptionsBuilder configures the Search Options
    /// </summary>
    public class SearcherOptionsBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the SearcherOptions
        /// </summary>
        protected SearcherOptions<T> SearcherOptions { get; set; }

        public SearcherOptionsBuilder(SearcherOptions<T> searchOptions)
        {
            this.SearcherOptions = searchOptions;
        }

        /// <summary>
        /// Adds a Template Restriction to filter the results on
        /// </summary>
        /// <param name="id">The Template ID</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> AddRestriction(ID id)
        {
            this.SearcherOptions.Restrictions.Add(id);
            return this;
        }

        /// <summary>
        /// Adds a Template Restriction to filter the results on
        /// </summary>
        /// <param name="ids">The Template ID</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> AddRestriction(params ID[] ids)
        {
            if (ids != null && ids.Any())
            {
                ids.ForEach(id => this.SearcherOptions.Restrictions.Add(id));
            }

            return this;
        }

        /// <summary>
        /// Adds a field name to the facets
        /// </summary>
        /// <param name="facet">Field name within the index</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> AddFacet(string facet)
        {
            Assert.ArgumentNotNullOrEmpty(facet, "facet");

            this.SearcherOptions.Facets.Add(facet);
            return this;
        }

        /// <summary>
        /// Sets the Search Manager to be used for Searching
        /// </summary>
        /// <param name="manager">Search Manager</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SetSearchManager(SearchManager manager)
        {
            Assert.ArgumentNotNull(manager, "manager");

            this.SearcherOptions.SearchManager = manager;
            return this;
        }

        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="pageMode">pageMode of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SetPageMode(PageMode pageMode)
        {
            this.SearcherOptions.PageMode = pageMode;
            return this;
        }

        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="page">Page of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SetPage(int page)
        {
            this.SearcherOptions.Start = page <= 0 ? 1 : page;
            return this;
        }

        /// <summary>
        /// Set the returned results page
        /// <para>Must set the PageMode to Start, otherwise sets the Paging inaccurately</para>
        /// </summary>
        /// <param name="start">Page of the results</param>
        /// <param name="includeStart">Should the results include the starting number</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SetStartingPosition(int start, bool includeStart = false)
        {
            var i = start;
            
            // Ensure we are working with a 
            if (i < 0) i = 0; 

            this.SearcherOptions.Start = ((i > 0) && includeStart)
                ? i - 1
                : i;

            return this;
        }

        /// <summary>
        /// Sets the Display Size of the Returned Results
        /// </summary>
        /// <param name="display"></param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SetDisplaySize(int display)
        {
            this.SearcherOptions.Display = display;
            return this;
        }


        /// <summary>
        /// Sets the Display Size of the Returned Results
        /// </summary>
        /// <param name="display"></param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> Set(int display)
        {
            this.SearcherOptions.Display = display;
            return this;
        }

        /// <summary>
        /// Sorts the Search Results
        /// </summary>
        /// <param name="sortOrder">Search Sort Order</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public SearcherOptionsBuilder<T> SortOrder(SortOrder sortOrder)
        {
            this.SearcherOptions.SortOrder = sortOrder;

            return this;
        }
    }
}