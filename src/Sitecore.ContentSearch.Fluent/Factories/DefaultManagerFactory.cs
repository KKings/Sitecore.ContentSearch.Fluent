namespace Sitecore.ContentSearch.Fluent.Factories
{
    using System.Collections.Generic;
    using Providers;
    using Repositories;
    using Results;
    using Services;
    using Sitecore.Abstractions;

    public class DefaultManagerFactory : IManagerFactory
    {
        private readonly IDatabaseProvider databaseProvider;
        private readonly IQueryService queryService;
        private readonly BaseLog logger;

        public DefaultManagerFactory(IDatabaseProvider databaseProvider, IQueryService queryService, BaseLog logger)
        {
            this.databaseProvider = databaseProvider;
            this.queryService = queryService;
            this.logger = logger;
        }

        public virtual ISearchManager<T> Create<T>(IDictionary<string, string> indexes) where T : SearchResultItem
        {
            var indexProvider = new DefaultIndexProvider(this.databaseProvider, indexes);

            var searchManager = new DefaultSearchManager(
                new DefaultSearchProvider(
                    new ContentSearchRepository(indexProvider), this.queryService, this.logger));

            return new DefaultSearchManager<T>(searchManager);
        }
    }
}
