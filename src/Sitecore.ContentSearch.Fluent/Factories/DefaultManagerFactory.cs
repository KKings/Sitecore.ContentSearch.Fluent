namespace Sitecore.ContentSearch.Fluent.Factories
{
    using System.Collections.Generic;
    using Providers;
    using Repositories;
    using Results;
    using Services;

    public class DefaultManagerFactory : IManagerFactory
    {
        private readonly IDatabaseProvider databaseProvider;
        private readonly IQueryService queryService;

        public DefaultManagerFactory(IDatabaseProvider databaseProvider, IQueryService queryService)
        {
            this.databaseProvider = databaseProvider;
            this.queryService = queryService;
        }

        public virtual ISearchManager<T> Create<T>(IDictionary<string, string> indexes) where T : SearchResultItem
        {
            var indexProvider = new DefaultIndexProvider(this.databaseProvider, indexes);

            var searchManager = new DefaultSearchManager(
                new DefaultSearchProvider(
                    new ContentSearchRepository(indexProvider), this.queryService));

            return new DefaultSearchManager<T>(searchManager);
        }
    }
}
