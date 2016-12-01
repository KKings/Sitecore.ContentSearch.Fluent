// <copyright file="FilterOptionsQueryBuilder.cs" company="Kyle Kingsbury">
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
    /// Builds the 
    /// </summary>
    public class FilterQueryBuilder<T> : SearchBuilderBase<T> where T : SearchResultItem
    {
        public FilterQueryBuilder(FilterOptions<T> filterOptions) : base(filterOptions) { }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public override SearchBuilderBase<T> And(Action<SearchBuilderBase<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>();

            filterAction(new FilterQueryBuilder<T>(filterOptions));

            if (filterOptions.Filter == null)
            {
                return this;
            }

            this.Options.Filter = this.Options.Filter != null 
                ? this.Options.Filter.And(filterOptions.Filter)
                : PredicateBuilder.True<T>().And(filterOptions.Filter);

            return this;
        }

        /// <summary>
        /// Groups filters by AND
        /// </summary>
        /// <returns>Instance of the SearchQueryOptionsBuilder</returns>
        public override SearchBuilderBase<T> Or(Action<SearchBuilderBase<T>> filterAction)
        {
            var filterOptions = new FilterOptions<T>();

            filterAction(new FilterQueryBuilder<T>(filterOptions));

            if (filterOptions.Filter == null)
            {
                return this;
            }

            this.Options.Filter = this.Options.Filter != null 
                ? this.Options.Filter.Or(filterOptions.Filter)
                : PredicateBuilder.False<T>().Or(filterOptions.Filter);

            return this;
        }
    }
}
