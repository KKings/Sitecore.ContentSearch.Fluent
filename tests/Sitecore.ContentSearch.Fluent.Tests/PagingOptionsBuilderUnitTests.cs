using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using Builders;
    using Options;

    [TestClass]
    public class PagingOptionsBuilderUnitTests
    {
        [TestMethod]
        public void Skipped_Start_Equals_StartingPosition()
        {
            // Assert
            var options = new PagingOptions { PageMode = PageMode.Start };
            var sut = new PagingOptionsBuilder<TestSearchResultItem>(options);
            var skip = 5;

            // Act
            sut.Skip(skip);

            // Arrange
            Assert.AreEqual(skip, options.StartingPosition);
        }

        [TestMethod]
        public void SetPage_Pager_Equals_StartingPosition()
        {
            // Assert
            var skip = 5;
            var size = 10;
            var options = new PagingOptions { PageMode = PageMode.Pager, Display = size};
            var sut = new PagingOptionsBuilder<TestSearchResultItem>(options);

            // Act
            sut.SetPage(skip);

            // Arrange
            Assert.AreEqual(40, options.StartingPosition);
        }
    }
}
