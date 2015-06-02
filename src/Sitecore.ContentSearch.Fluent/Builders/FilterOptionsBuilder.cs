// <copyright file="FilterOptionsBuilder.cs" company="Kyle Kingsbury">
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
    using Linq.Utilities;
    using Options;
    using Results;

    /// <summary>
    /// Todo: FilterOptionsBuilder Summary Description
    /// </summary>
    public class FilterOptionsBuilder<T> where T : SearchResultItem
    {        
        /// <summary>
        /// Gets or sets the QueryOptions
        /// </summary>
        protected FilterOptions<T> FilterOptions { get; set; }

        public FilterOptionsBuilder(FilterOptions<T> filterOptions)
        {
            this.FilterOptions = filterOptions;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsBuilder<T> And(Action<FilterOptionsQueryBuilder<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>();

            filterAction(new FilterOptionsQueryBuilder<T>(filterOptions));

            this.FilterOptions.Filter = this.FilterOptions.Filter.And(filterOptions.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results if available
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterOptionsBuilder<T> Or(Action<FilterOptionsQueryBuilder<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>(false);

            filterAction(new FilterOptionsQueryBuilder<T>(filterOptions));

            this.FilterOptions.Filter = this.FilterOptions.Filter.Or(filterOptions.Filter);

            return this;
        }
    }
}
