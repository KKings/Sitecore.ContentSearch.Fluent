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
    /// Builds the Filter Options
    /// </summary>
    public sealed class FilterBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the FilterOptions
        /// </summary>
        public FilterOptions<T> FilterOptions { get; }

        public FilterBuilder(FilterOptions<T> filterOptions)
        {
            this.FilterOptions = filterOptions;
        }

        /// <summary>
        /// Filters out the Search Results
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterBuilder<T> And(Action<FilterQueryBuilder<T>> filterAction)
        {
            var options = new FilterOptions<T>();

            filterAction(new FilterQueryBuilder<T>(options));

            if (options.Filter == null)
            {
                return this;
            }

            this.FilterOptions.Filter = this.FilterOptions.Filter != null
                ? this.FilterOptions.Filter.And(options.Filter)
                : PredicateBuilder.True<T>().And(options.Filter);

            return this;
        }

        /// <summary>
        /// Filters out the Search Results if available
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public FilterBuilder<T> Or(Action<FilterQueryBuilder<T>> filterAction)
        {
            var options = new FilterOptions<T>();

            filterAction(new FilterQueryBuilder<T>(options));


            if (options.Filter == null)
            {
                return this;
            }

            this.FilterOptions.Filter = this.FilterOptions.Filter != null
                ? this.FilterOptions.Filter.Or(options.Filter)
                : PredicateBuilder.False<T>().Or(options.Filter);

            return this;
        }
    }
}
