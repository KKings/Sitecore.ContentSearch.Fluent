namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using ContentSearch;
    using Fluent;
    using Facets;
    using Linq;
    using Linq.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class SearchManagerUnitTests
    {
        private Mock<SearchManager> searchManager;


        [TestInitialize]
        public void Setup()
        {
            var index = TestSearchResultIndexFactory.CreateIndex();

            // create the mock context
            var searchContextMock = new Mock<IProviderSearchContext>();
            searchContextMock.Setup(c => c.GetQueryable<TestSearchResultItem>())
                             .Returns(index);

            var indexProvider = new Mock<IIndexProvider>();
            indexProvider.SetupGet(m => m.SearchContext)
                         .Returns(searchContextMock.Object);

            var manager = new Mock<SearchManager>(indexProvider.Object);

            var searcher = new Mock<DefaultSearcher<TestSearchResultItem>>(index);
            searcher.Setup(
                m =>
                    m.Filter(
                        It.IsAny<IQueryable<TestSearchResultItem>>(),
                        It.IsAny<Expression<Func<TestSearchResultItem, bool>>>()))
                    .Returns<IQueryable<TestSearchResultItem>, Expression<Func<TestSearchResultItem, bool>>>(
                        (items, expression) => items.Where(expression));


            searcher.Setup(m => m.GetFacets(It.IsAny<IQueryable<TestSearchResultItem>>(), It.IsAny<IList<IFacetOn>>()))
                    .Returns(new FacetResults());

            searcher.Setup(m => m.GetResults(It.IsAny<IQueryable<TestSearchResultItem>>()))
                    .Returns<IQueryable<TestSearchResultItem>>(
                        (items =>
                            new SearchResults<TestSearchResultItem>(
                                items.Select(i => new SearchHit<TestSearchResultItem>(0, i)), items.Count())));

            searcher.CallBase = true;
            manager.CallBase = true;

            manager.Setup(m => m.GetSearcher<TestSearchResultItem>())
                   .Returns(searcher.Object);
            manager.Setup(m => m.GetQueryable<TestSearchResultItem>())
                   .Returns(index);

            this.searchManager = manager;

        }

        [TestMethod]
        public void Query_And_Where_ReturnsResultsWithTest()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .Where(result => result.Name.Contains("Test")))));

                Assert.AreEqual(6, results.Total);
            }
        }

        [TestMethod]
        public void Query_Or_Where_ReturnsOnlyArticlesWithTest()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .Where(result => result.Name.Contains("Test"))
                            .Where(result => result.TemplateId == Constants.ArticleTemplateId))));

                Assert.AreEqual(2, results.Total);
            }
        }


        [TestMethod]
        public void Combined_Query_MultipleOrWhere_Filter_Where_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .OrWhere(result => result.Semantics.Contains(Constants.SemanticId1))
                            .OrWhere(result => result.Semantics.Contains(Constants.SemanticId5))))
                    .Filter(filter => filter
                        .And(group => group
                            .Where(result => result.Language == "en-US"))));

                Assert.AreEqual(2, results.Total);
            }
        }

        [TestMethod]
        public void Query_Complex_Multiple_Conditions_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .Where(result => result.Semantics.Contains(Constants.SemanticId1))
                            .Where(result => result.Name.Contains("Test")))
                        .Or(group => group
                            .Or(or => or
                                .Where(result => result.Name == "Test Result Item # 2")
                                .Where(result => result.Name == "Test Result Item # 2"))
                            .OrWhere(result => result.Name.Contains("kyle"))
                            .Where(result => result.TemplateId == Constants.ArticleTemplateId))));

                Assert.AreEqual(4, results.Total);
            }
        }


        [TestMethod]
        public void Query_Any_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .Any(new[] { "Test", "Visual", "Studio" }, (result, s) => result.Name.Contains(s)))));

                Assert.AreEqual(6, results.Total);
            }
        }

        [TestMethod]
        public void Query_All_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .All(new[] { "Test", "Visual", "Studio" }, (result, s) => result.Name.Contains(s)))));


                Assert.AreEqual(0, results.Total);
            }
        }

        [TestMethod]
        public void Query_Complex_Any_OrWhere_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .Any(new[] { "1", "6", "Studio" }, (result, special) => result.Name.Contains(special))
                            .OrWhere(result => result.TemplateId == Constants.ArticleTemplateId))));

                Assert.AreEqual(4, results.Total);
            }
        }


        [TestMethod]
        public void Filter_Any_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Filter(q => q
                        .And(group => group
                            .Any(new[] { "1", "2", "Studio" }, (result, special) => result.Name.Contains(special)))));

                Assert.AreEqual(4, results.Total);
            }
        }

        [TestMethod]
        public void Filter_All_ReturnsResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Filter(q => q
                        .And(group => group
                            .All(new[] { "Test", "Result", "1" }, (result, s) => result.Name.Contains(s)))));

                Assert.AreEqual(2, results.Total);
            }
        }

        [TestMethod]
        public void Query_IfWhere_ReturnsAllResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Query(q => q
                        .And(group => group
                            .IfWhere(false, result => result.Name == "Item Not Found"))));

                Assert.AreEqual(6, results.Total);
            }
        }

        [TestMethod]
        public void Filter_IfWhere_ReturnsAllResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Filter(q => q
                        .And(group => group
                            .IfWhere(false, result => result.Name == "Item Not Found"))));

                Assert.AreEqual(6, results.Total);
            }
        }

        [TestMethod]
        public void SearchManager_Empty_ReturnsAllResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => {});

                Assert.AreEqual(6, results.Total);
            }
        }

        [TestMethod]
        public void Take_4_NoQuery_ReturnsExactResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Paging(o => o
                        .Take(4)));

                Assert.AreEqual(4, results.Total);
            }
        }

        [TestMethod]
        public void Take_0_NoQuery_ReturnsAllResults()
        {
            using (var manager = this.searchManager.Object)
            {
                var results = manager.ResultsFor<TestSearchResultItem>(search => search
                    .Paging(o => o
                        .Take(0)));

                Assert.AreEqual(6, results.Total);
            }
        }
    }
}
