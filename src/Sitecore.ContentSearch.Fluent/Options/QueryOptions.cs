// <copyright file="QueryOptions.cs" company="Kyle Kingsbury">
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
    using System.Linq;
    using System.Linq.Expressions;
    using Linq.Utilities;
    using Results;

    /// <summary>
    /// QueryOptions Summary
    /// </summary>
    public class QueryOptions<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the Filter expressions
        /// <para>Always set the filter expression when added an expression</para>
        /// </summary>
        public Expression<Func<T, bool>> Filter { get; set; }

        /// <summary>
        /// Gets or sets the Queryable
        /// </summary>
        public IQueryable<T> Queryable { get; internal set; }

        /// <summary>
        /// Gets or sets if its an Or Predicate
        /// </summary>
        public bool UseAndPredicate { get; internal set; }

        public QueryOptions() : this(true)
        {
        }

        public QueryOptions(bool isAnd): this(null, isAnd)
        {
        }

        public QueryOptions(bool isAnd, bool useAnd) : this(null, isAnd, useAnd)
        {
        }

        public QueryOptions(IQueryable<T> queryable, bool isAnd = true, bool useAndPredicate = true)
        {
            this.UseAndPredicate = useAndPredicate;

            this.Queryable = queryable;
            this.Filter = (isAnd) 
                ? PredicateBuilder.True<T>()
                : PredicateBuilder.False<T>();
        }
    }
}