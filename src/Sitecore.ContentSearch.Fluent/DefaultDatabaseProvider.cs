// <copyright file="Searcher.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent
{
    using Configuration;
    using Data;

    public class DefaultDatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// Default Database Name if there is no Context
        /// </summary>
        public virtual string DefaultDatabaseName => "web";

        /// <summary>
        /// Context Database for the Index
        /// </summary>
        public virtual Database Context
        {
            get { return Sitecore.Context.Database ?? this.DefaultFactory.GetDatabase(this.DefaultDatabaseName); }
        }

        /// <summary>
        /// Default Configuration Factory
        /// </summary>
        public readonly DefaultFactory DefaultFactory;

        public DefaultDatabaseProvider(DefaultFactory defaultFactory)
        {
            this.DefaultFactory = defaultFactory;
        }
    }
}
