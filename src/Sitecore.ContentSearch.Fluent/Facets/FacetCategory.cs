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
namespace Sitecore.ContentSearch.Fluent.Facets
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Facet by Name
    /// </summary>
    public class FacetCategory
    {
        /// <summary>
        /// Gets or sets the Name of the Facet
        /// <para>Note, this is the name within the index</para>
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Gets of sets the Facet Values
        /// </summary>
        public virtual IList<FacetValue> Values { get; }

        public FacetCategory(string name, IList<FacetValue> values)
        {
            this.Name = name;
            this.Values = values ?? new FacetValue[0];
        }
    }
}