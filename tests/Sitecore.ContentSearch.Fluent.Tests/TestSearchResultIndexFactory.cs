
namespace Sitecore.ContentSearch.Fluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TestSearchResultIndexFactory
    {
        public static IQueryable<TestSearchResultItem> CreateIndex()
        {
            return new List<TestSearchResultItem>
            {
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Language = "en-US",
                    Semantics = new []{ Constants.SemanticId1, Constants.SemanticId2, Constants.SemanticId3 },
                    Name = "Test Result Item # 1",
                    TemplateId = Constants.EventTemplateId
                },
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Language = "es-ES",
                    Semantics = new []{ Constants.SemanticId1, Constants.SemanticId2, Constants.SemanticId3 },
                    Name = "Test Result Item # 1",
                    TemplateId = Constants.EventTemplateId
                },
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Language = "en-US",
                    Semantics = new []{ Constants.SemanticId3, Constants.SemanticId4 },
                    Name = "Test Result Item # 2",
                    TemplateId = Constants.ArticleTemplateId
                },
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Language = "es-ES",
                    Semantics = new []{ Constants.SemanticId3, Constants.SemanticId4 },
                    Name = "Test Result Item # 2",
                    TemplateId = Constants.ArticleTemplateId
                },
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    Language = "en-US",
                    Semantics = new []{ Constants.SemanticId5 },
                    Name = "Test Result Item # 3",
                    TemplateId = Constants.CategoryTemplateId
                },
                new TestSearchResultItem
                {
                    Content = "Spicy jalapeno chicken jowl burgdoggen irure bresaola minim corned beef nostrud anim enim. Elit turkey corned beef sunt shank tempor. Duis veniam leberkas mollit swine do, ad ex t-bone drumstick. Bacon incididunt consequat, corned beef sed burgdoggen consectetur.",
                    CreatedBy = "Kyle Kingsbury",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    Language = "en-ES",
                    Semantics = new []{ Constants.SemanticId5 },
                    Name = "Test Result Item # 3",
                    TemplateId = Constants.CategoryTemplateId
                },

            }.AsQueryable();
        }
    }
}
