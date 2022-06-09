using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System.Linq;
    using Builders;
    using Options;

    /// <summary>
    /// Summary description for SortingOptionsBuilderUnitTests
    /// </summary>
    [TestClass]
    public class SortingOptionsBuilderUnitTests
    {
        [TestCategory("sorting")]
        [TestMethod]
        public void SortingBuilder_ForExpressionUsingNull_DoesNotThrowException()
        {
            // Arrange
            var options = new SortingOptions<TestSearchResultItem>();
            var builder = new SortingOptionsBuilder<TestSearchResultItem>(options);

            // Act
            builder.By(result => null);

            // Assert
        }

        [TestCategory("sorting")]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void SortingBuilder_ForNullExpression_ThrowsException()
        {
            // Arrange
            var options = new SortingOptions<TestSearchResultItem>();
            var builder = new SortingOptionsBuilder<TestSearchResultItem>(options);

            // Act
            builder.By(null);

            // Assert
        }

        [TestCategory("sorting")]
        [TestMethod]
        public void SortingBuilder_ForExpression_AddsExpression()
        {
            // Arrange
            var options = new SortingOptions<TestSearchResultItem>();
            var builder = new SortingOptionsBuilder<TestSearchResultItem>(options);

            // Act
            builder.By(result => result.Name, SortOrder.Descending);

            // Assert
            Assert.IsTrue(options.Operations.Count == 1);
            Assert.IsTrue(options.Operations.First().SortOrder == SortOrder.Descending);
        }
    }
}
