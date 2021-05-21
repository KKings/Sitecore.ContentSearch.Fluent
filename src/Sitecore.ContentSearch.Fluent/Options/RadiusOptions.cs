// MIT License
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
    using System.Linq.Expressions;
    using Data;
    using Results;

    public class RadiusOptions<T> where T : SearchResultItem
    {
        /// <summary>
        /// Get or sets the Expression
        /// </summary>
        public virtual Expression<Func<T, Coordinate>> Expression { get; set; }

        /// <summary>
        /// Gets or sets the Latitude
        /// </summary>
        public double Latitude { get; set; } = Double.NaN;

        /// <summary>
        /// Gets or sets the Longitude
        /// </summary>
        public double Longitude { get; set; } = Double.NaN;

        /// <summary>
        /// Gets or sets the Distance
        /// </summary>
        public double Distance { get; set; } = Double.NaN;

        /// <summary>
        /// Gets or sets if Solr should order by distance
        /// </summary>
        public bool? OrderByDistance { get; set; }

        /// <summary>
        /// Gets or sets if Solr should order by distance descending
        /// </summary>
        public bool? OrderByDistanceDescending { get; set; }

        /// <summary>
        /// Gets or sets if Solr should calculate the distance by using Bbox
        /// </summary>
        public bool UseBox { get; set; } = false;
    }
}
