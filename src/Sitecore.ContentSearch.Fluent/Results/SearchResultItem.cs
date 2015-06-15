// <copyright file="SearchResultItem.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Results
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using Converters;
    using Data;

    /// <summary>
    /// SearchResultItem Summary
    /// </summary>
    public abstract class SearchResultItem : ISearchResultItem
    {
        /// <summary>
        /// The fields within the Search Result
        /// </summary>
        private readonly IDictionary<string, object> _fields = new Dictionary<string, object>();

        [IndexField("_language")]
        public string Language { get; set; }

        [XmlIgnore]
        [IndexField("_uniqueid")]
        [TypeConverter(typeof(IndexFieldItemUriValueConverter))]
        public ItemUri Uri { get; set; }

        [IndexField("__smallcreateddate")]
        public DateTime CreatedDate { get; set; }

        [IndexField("_template")]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public ID TemplateId { get; set; }

        [IndexField("_name")]
        public string Name { get; set; }

        [IndexField("_group")]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public ID Id { get; set; }

        /// <summary>
        /// Custom Indexer to get the fields
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                return this._fields[key.ToLowerInvariant()].ToString();
            }

            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                this._fields[key.ToLowerInvariant()] = value;
            }
        }
    }
}