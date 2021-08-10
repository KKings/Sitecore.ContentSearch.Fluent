namespace Sitecore.ContentSearch.Fluent
{
    using System;
    using System.Linq;
    using Builders;
    using Results;

    public interface ISearchManager<T> : IDisposable where T : SearchResultItem
    {
        /// <summary>
        /// Results for a Search Builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchResults<T> ResultsFor(Action<ISearcherBuilder<T>> searcherBuilder);

        /// <summary>
        /// Facet Results for a SearchBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchFacetResults FacetsFor(Action<ISearcherBuilder<T>> searcherBuilder);

        /// <summary>
        /// Facet Results for a SearchBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchResultsWithFacets<T> ResultsWithFacetsFor(Action<ISearcherBuilder<T>> searcherBuilder);

        /// <summary>
        /// Gets the configuration object
        /// </summary>
        /// <param name="searcherBuilder"></param>
        /// <returns></returns>
        SearchConfiguration<T> Build(Action<ISearcherBuilder<T>> searcherBuilder);

        /// <summary>
        /// Gets the facets
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        SearchFacetResults FacetsFor(SearchConfiguration<T> configuration);

        /// <summary>
        /// Gets the configuration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        SearchResults<T> ResultsFor(SearchConfiguration<T> configuration);

        /// <summary>
        /// Gets the IQueryable before it is executed
        /// </summary>
        IQueryable<T> GetQueryable(Action<ISearcherBuilder<T>> searcherBuilder);
    }
}
