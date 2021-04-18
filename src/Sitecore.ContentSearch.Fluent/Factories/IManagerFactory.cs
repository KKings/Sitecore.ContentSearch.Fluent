namespace Sitecore.ContentSearch.Fluent.Factories
{
    using System.Collections;
    using System.Collections.Generic;
    using Linq.Extensions;
    using Results;

    public interface IManagerFactory
    {
        ISearchManager<T> Create<T>(IDictionary<string, string> indexes) where T: SearchResultItem;
    }
}
