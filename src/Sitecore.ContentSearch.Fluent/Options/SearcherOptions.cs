﻿// MIT License
// 
// Copyright (c) 2016 Kyle Kingsbury
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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