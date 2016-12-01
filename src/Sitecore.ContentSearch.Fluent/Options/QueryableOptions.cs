// <copyright file="FilterOptions.cs" company="Kyle Kingsbury">
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
    using System.Linq.Expressions;
    using Results;

    public abstract class QueryableOptions<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the Filter expressions
        /// <para>Always set the filter expression when added an expression</para>
        /// </summary>
        internal virtual Expression<Func<T, bool>> Filter { get; set; }
    }
}
