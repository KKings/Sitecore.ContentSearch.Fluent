using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sitecore.ContentSearch.Fluent.Tests
{
    using Builders;
    using Options;

    [TestClass]
    public class WithinRadiusBuilderUnitTests
    {
        [TestMethod]
        public void WithinRadiusBuilder_ForOptionsSet_OptionsSet()
        {
            // Assert
            var options = new RadiusOptions<TestSearchResultItem>();

            var sut = new WithinRadiusBuilder<TestSearchResultItem>(options);
            var distance = 100;
            var longitude = 180;
            var latitude = 90;

            // Act
            sut.Within(m => m.Location, latitude, longitude, distance);

            // Arrange
            Assert.AreEqual(distance, options.Distance);
            Assert.AreEqual(latitude, options.Latitude);
            Assert.AreEqual(longitude, options.Longitude);
        }
    }
}
