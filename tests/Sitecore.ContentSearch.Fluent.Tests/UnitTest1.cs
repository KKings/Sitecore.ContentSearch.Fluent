using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System;
    using global::Sitecore.ContentSearch.Linq;
    using global::Sitecore.ContentSearch.Linq.Utilities;
    using global::Sitecore.Data;
    using Results;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var manager = new SearchManager("a", "b"))
            {
                var results = manager.ResultsFor<SearchResultItem>(search => search
                    .Options(o => o
                        .AddRestriction(ID.NewID)
                        .SetDisplaySize(15))
                    .Query(q => q
                        .And(and => and
                            .Where(new String[0], (expression, s) => expression.Or(r => r.Language == s))))
                    .Sort(sort => sort
                        .By(result => result.CreatedDate, SortOrder.Ascending)));
            }
        }
    }
}
