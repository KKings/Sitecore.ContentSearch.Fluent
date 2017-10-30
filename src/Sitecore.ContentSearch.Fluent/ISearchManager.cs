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
namespace Sitecore.ContentSearch.Fluent
{
    using System;
    using Builders;
    using Facets;
    using Results;

    public interface ISearchManager : IDisposable
    {
        /// <summary>
        /// Results for a Search Builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchResults<T> ResultsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder) where T : SearchResultItem;

        /// <summary>
        /// Facet Results for a SearchBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchFacetResults FacetsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder) where T : SearchResultItem;

        /// <summary>
        /// Facet Results for a SearchBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchResultsWithFacets<T> ResultsWithFacetsFor<T>(Action<ISearcherBuilder<T>> searcherBuilder) where T : SearchResultItem;

        /// <summary>
        /// Gets the configuration object
        /// </summary>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchConfiguration<T> Build<T>(Action<ISearcherBuilder<T>> searcherBuilder) where T : SearchResultItem;

        /// <summary>
        /// Gets the facets
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        SearchFacetResults FacetsFor<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;

        /// <summary>
        /// Gets the configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        SearchResults<T> ResultsFor<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;
    }
}