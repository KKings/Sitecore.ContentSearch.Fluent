namespace Sitecore.ContentSearch.Fluent
{
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;
    using Repositories;
    using Services;
    using DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IQueryService, QueryService>();
            serviceCollection.AddScoped<IResultRepository, ContentSearchRepository>();
            serviceCollection.AddScoped<IDatabaseProvider, DefaultDatabaseProvider>();
            serviceCollection.AddTransient<IIndexProvider, DefaultIndexProvider>(
                provider =>
                    new DefaultIndexProvider(provider.GetService<IDatabaseProvider>(),
                        new Dictionary<string, string> { { "master", "sitecore_master_index" }, { "web", "sitecore_web_index" } }));
            serviceCollection.AddScoped<ISearchProvider, DefaultSearchProvider>();
            serviceCollection.AddTransient<ISearchManager, DefaultSearchManager>();
        }
    }
}
