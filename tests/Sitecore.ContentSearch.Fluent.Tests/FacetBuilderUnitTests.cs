

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System;
    using System.Linq;
    using Builders;
    using Facets;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Options;

    [TestClass]
    public class FacetBuilderUnitTests
    {
        [TestCategory("facet")]
        [TestMethod]
        public void FacetBuilder_ForAddingByString_ShouldAddByString()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On("_content");

            // Assert
            Assert.IsTrue(options.Facets.Any(f => f.Key == "_content"));
        }

        [TestCategory("facet")]
        [TestMethod]
        public void FacetBuilder_ForAddingByEmptyString_ShouldNotAdd()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On(String.Empty);

            // Assert
            Assert.IsTrue(options.Facets.Count == 0);
        }

        [TestCategory("facet")]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void FacetBuilder_ForAddingByNullFacetOn_ShouldThrowException()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On((FacetOn)null);

            // Assert
        }

        [TestCategory("facet")]
        [TestMethod]
        public void FacetBuilder_ForAddingByExpression_ShouldAddByIndexField()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On(result => result.Content);

            // Assert
            Assert.IsTrue(options.Facets.Any(f => f.Key == "_content"));
        }

        [TestCategory("facet")]
        [TestMethod]
        public void FacetBuilder_ForAddingByListOfExpressions_ShouldAddByIndexField()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On(result => result.Content, result => result.TemplateName, result => result.TestingFacetKey);

            // Assert
            Assert.IsTrue(options.Facets.Any(f => f.Key == "_content"));
            Assert.IsTrue(options.Facets.Any(f => f.Key == "_templatename"));
            Assert.IsTrue(options.Facets.Any(f => f.Key == "testingfacetkey"));
        }

        [TestCategory("facet")]
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void FacetBuilder_ForAddingByListOfExpressionsWithNull_ShouldThrowException()
        {
            // Arrange
            var options = new FacetOptions();
            var facetBuilder = new FacetBuilder<TestSearchResultItem>(options);

            // Act
            facetBuilder.On(result => result.Content, 
                result => null, // Throw Exception
                result => result.TestingFacetKey);

            // Assert
        }
    }
}
