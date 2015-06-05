using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System;
    using System.Linq;
    using global::Sitecore.ContentSearch.Linq;
    using global::Sitecore.ContentSearch.Linq.Utilities;
    using global::Sitecore.Data;
    using Results;
    using Search;
    using SearchManager = Fluent.SearchManager;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //using (var manager = new SearchManager("sitecore_master_index", "sitecore_web_index"))
            //{
            //    var results = manager.ResultsFor<SearchResultItem>(search => search
            //        .Options(o => o
            //            .SetDisplaySize(15)
            //            .AddRestriction(ArticlePage.TemplateId))
            //        .Query(q => q
            //            .And(and => and
            //                .Where(result => result.Name.Like("test"))))
            //        .Sort(sort => sort
            //            .By(result => result.CreatedDate)));
            //}

            //using (var context = ContentSearchManager.GetIndex("sitecore_web_index").CreateSearchContext())
            //{
            //    var predicate = PredicateBuilder.True<SearchResultItem>();

            //    predicate = predicate.Or(result => result.TemplateId == ArticlePage.TemplateId);

            //    var query = context.GetQueryable<SearchResultItem>().Filter(predicate);

            //    query = query.Take(15);
            //    query = query.Where(i => i.Name.Like("test"));
            //    query = query.OrderBy(result => result.CreatedDate);

            //    var results = query.GetResults();
            //}
        }
    }
}
