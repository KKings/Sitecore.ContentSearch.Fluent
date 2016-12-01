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
    using Options;
    using Results;

    /// <summary>
    /// SearcherOptionsBuilder configures the Search Options
    /// </summary>
    public class PagingOptionsBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Builds the SearcherOptions
        /// </summary>
        protected readonly SearcherOptions<T> SearcherOptions;

        public PagingOptionsBuilder(SearcherOptions<T> searchOptions)
        {
            this.SearcherOptions = searchOptions;
        }
        
        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="pageMode">pageMode of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetPageMode(PageMode pageMode)
        {
            this.SearcherOptions.PageMode = pageMode;
            return this;
        }

        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="page">Page of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetPage(int page)
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
        public PagingOptionsBuilder<T> Skip(int start, bool includeStart = false)
        {
            var temp = start;
            
            // Ensure we are working with a valid number
            if (temp < 0) temp = 0; 

            this.SearcherOptions.Start = (temp > 0) && includeStart
                ? temp - 1
                : temp;

            return this;
        }

        /// <summary>
        /// Sets the Display Size of the Returned Results
        /// </summary>
        /// <param name="display"></param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> Take(int display)
        {
            this.SearcherOptions.Display = display;
            return this;
        }
    }
}