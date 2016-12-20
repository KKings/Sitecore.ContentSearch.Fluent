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
namespace Sitecore.ContentSearch.Fluent.Builders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;
    using Facets;
    using Options;
    using Results;

    public class FacetBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Gets or sets the FacetOptions
        /// </summary>
        public FacetOptions FacetOptions { get; }

        public FacetBuilder(FacetOptions facetOptions)
        {
            this.FacetOptions = facetOptions;
        }

        /// <summary>
        /// Adds the Facet in the order it was 
        /// </summary>
        /// <param name="property">The property key to facet on</param>
        /// <returns><see cref="FacetBuilder{T}"/></returns>
        public FacetBuilder<T> On(string property)
        {
            if (String.IsNullOrEmpty(property))
            {
                return this;
            }

            var facet = new FacetOn(property);

            this.On(facet);

            return this;
        }

        /// <summary>
        /// Adds the Facet in the order it was received
        /// </summary>
        /// <param name="facetOn">The Facet</param>
        /// <exception cref="ArgumentNullException">Argument cannot be null</exception>
        /// <returns><see cref="FacetBuilder{T}"/></returns>
        public FacetBuilder<T> On(FacetBase facetOn)
        {
            if (facetOn == null)
            {
                throw new ArgumentNullException(nameof(facetOn));
            }

            this.FacetOptions.Facets.Add(facetOn);

            return this;
        }

        /// <summary>
        /// Adds the Facet by a property of the <see cref="T"/> of <see cref="SearchResultItem"/>
        /// <para>If the <see cref="IndexFieldAttribute"/> does not exist on the property, 
        /// the default will be a lowercase name of the property</para>
        /// </summary>
        /// <param name="expression">The expression of the property</param>
        /// <exception cref="ArgumentNullException">Expression cannot be null</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a method</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a field</exception>
        /// <returns><see cref="FacetBuilder{T}"/></returns>
        public FacetBuilder<T> On<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            // Extension to convert the expression into the Property needed
            var propInfo = expression.ToPropertyInfo();

            /**
             * Attempt to get the IndexFieldAttribute on the Property, if it 
             * doesn't exist, just take the name of the property
             */
            var indexField = propInfo.GetCustomAttribute<IndexFieldAttribute>();

            var facetKey = indexField != null ? indexField.IndexFieldName : propInfo.Name.ToLower();

            var facetOn = new FacetOn(facetKey);

            return this.On(facetOn);
        }

        /// <summary>
        /// Adds facets by a list of expressions, <see cref="FacetBuilder{T}.On{TProperty}(Expression{Func{T, TProperty}}"/>
        /// <see>
        ///     <cref>On(Expression{Func{T, TProperty}})</cref>
        /// </see>
        ///     <para>If the <see cref="IndexFieldAttribute"/> does not exist on the property, 
        /// the default will be a lowercase name of the property</para>
        /// </summary>
        /// <param name="expressions">The expression of the property</param>
        /// <exception cref="ArgumentNullException">Expression cannot be null</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a method</exception>
        /// <exception cref="ArgumentException">Expression cannot refer to a field</exception>
        /// <returns><see cref="FacetBuilder{T}"/></returns>
        public FacetBuilder<T> On<TProperty>(params Expression<Func<T, TProperty>>[] expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            foreach (var expression in expressions)
            {
                this.On(expression);
            }

            return this;
        }
    }
}