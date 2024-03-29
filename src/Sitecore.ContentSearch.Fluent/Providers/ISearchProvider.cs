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
namespace Sitecore.ContentSearch.Fluent.Providers
{
    using System;
    using System.Linq;
    using Builders;
    using Linq;
    using Results;
    using SolrNet;
    using SolrNet.Commands.Parameters;
    using SolrProvider;

    public interface ISearchProvider : IDisposable
    {
        IQueryable<T> GetQueryable<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;

        Results.SearchResults<T> GetResults<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;

        SearchFacetResults GetFacetResults<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;

        ISearcherBuilder<T> GetSearcherBuilder<T>(SearchConfiguration<T> configuration) where T : SearchResultItem;

        Results.SearchResults<T> ProcessResults<T>(Linq.SearchResults<T> results) where T : SearchResultItem;

        SearchFacetResults ProcessFacetResults(FacetResults results);

        SolrQueryResults<T> GetResults<T>(string query, QueryOptions queryOptions) where T : SearchResultItem;
    }
}