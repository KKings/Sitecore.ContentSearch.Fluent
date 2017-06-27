# Fluent API for Sitecore.ContentSearch

A lightweight, fluent API that reduces redundancy when using Sitecore's ContentSearch API in a unit testable manner.

### Features:
- Sorting
    - Multiple Sorting based on expressions
- Querying
    - Supports nested 'And' and 'Or' operations
- Filtering
    - Supports nested 'And' and 'Or' operations
- Pagination
- Projection (Select)
  - Ability to select only the fields needed

### Who is this library is for?

This library is designed for other developers, the implementers, to quickly build complex search functionality.

### Simple Example (Content Search vs Fluent API)
```c#
/**
 * Search for the first 15 article pages within the index with the 'Name' index field 
 * containing 'test', sorted by the Created Date, Ascending
 */
 
 // Using ContentSearch API
using (var context = ContentSearchManager.GetIndex("sitecore_web_index").CreateSearchContext())
{
    var predicate = PredicateBuilder.True<SearchResultItem>();
    
    predicate = predicate.Or(result => result.TemplateId == ArticlePage.TemplateId);
    
    var query = context.GetQueryable<SearchResultItem>().Filter(predicate);
    
    query = query.Take(15);
    query = query.Where(i => i.Name.Like("test"));
    query = query.OrderBy(result => result.CreatedDate);
    
    var results = query.GetResults();
}

/**
 * This example skips over the Dependency Injection of all services.
 * See the Getting Started Guide
 */ 

var provider = ServiceLocator.ServiceProvider.GetService<ISearchProvider>();

using (var manager = new DefaultSearchManager(provider))
{
    var results = manager.ResultsFor<SearchResultItem>(search => search
        .Paging(paging => paging
            .Take(15))
        .Query(q => q
            .And(and => and
                .Where(result => result.Name.Like("test"))))
        .Filter(filter => filter
            .And(and => and
                .Where(result => result.TemplateId == ArticlePage.TemplateId)))
        .Sort(sort => sort
            .By(result => result.CreatedDate)));
}
```

Not much change for simple examples, but its still not unit testable.

But what if we have filters that need to be taken into account?

```c#
/**
 * Get the 2nd page of articles within the index, filtered by articles matching all tags (if available) similar to facets,
 * filtered by if the result is a Featured Article (only if selected),
 * sorted by the created date
 */

// Setting up test data
var tags = new List<string> { "Tag1", "Tag2" };
var isFeature = false;

using (var context = ContentSearchManager.GetIndex("sitecore_web_index").CreateSearchContext())
{
    var predicate = PredicateBuilder.True<SearchResultItem>();

    predicate = predicate.Or(result => result.TemplateId == ArticlePage.TemplateId);

    var query = context.GetQueryable<SearchResultItem>().Filter(predicate);

    query = query.Page(2, 15);

    // Filter by the tags
    if (tags.Any())
    {
        var tagsPredicate = tags.Aggregate(PredicateBuilder.True<SearchResultItem>(), (current, tag) => current.And(query.Where(result => result.Tags.Contains(tag))));

        query = query.Where(tagsPredicate);
    }

    // Filter by if the article is a feature
    if (isFeature)
    {
        query = query.Where(result => result.IsFeature == true);
    }
               
    query = query.OrderBy(result => result.CreatedDate);

    var results = query.GetResults();
}

/**
 * This example skips over the Dependency Injection of all services.
 * See the Getting Started Guide
 */

var provider = ServiceLocator.ServiceProvider.GetService<ISearchProvider>();

using (var manager = new DefaultSearchManager(provider))
{
    var results = manager.ResultsFor<SearchResultItem>(search => search
        .Paging(paging => paging
            .SetPageMode(PageMode.Pager)
            .SetPage(2)
            .SetDisplaySize(15))
        .Query(q => q
            .And(and => and
                .IfAll(tags.Any(), tags, (result, tag) => result.Tags.Contains(tag))
                .IfWhere(isFeature, result => result.IsFeature == true)))
        .Filter(filter => filter
            .And(and => and
                .Where(result => result.TemplateId == ArticlePage.TemplateId)))
        .Sort(sort => sort
            .By(result => result.CreatedDate)));
}
```
Not only is the Fluent API easier to read, it makes more complex querying simple and intuitive.
No need to worry about the PredicateBuilder and what PredicateBuilder.False<SearchResultItem>() means.

### Query and Filter Methods

The query and filter methods support inherit from the same builder.

Clauses:

* **And** - Creates a Group using the PredicateBuilder.True<T>();
* **Or** - Creates a Group using the PredicateBuilder.False<T>();
* **Not** - Creates a logical where using Not
* **Where** - Adds a simple filter clause to the expression tree
* **OrWhere** - Adds a simple filter clause to the expression tree with a starting 'Or' expression
* **All** - All terms provided must return true against the condition
* **OrAll** -  All terms provided must return true against the condition with a starting Or expression
* **Any** -  Only one of the terms provided must return true against the condition
* **OrAny** - Only one of the terms provided must return true against the condition with a starting Or expression
* **ManyAny** - For each group of terms, only one term in the group must match against the condition, but each group must match at least one
* **OrManyAny** - For each group of terms, only one term in the group must match against the condition, but each group must match at least one with a starting Or expression

Conditional Clauses:

* **IfWhere** - If the condition is true, adds the condition using the *Where* method
* **IfOrWhere** - If the condition is true, adds the condition using the *OrWhere* method staring with an Or expression
* **IfAny** - If the condition is true, adds the condition using the *Any* method
* **IfOrAny** - If the condition is true, adds the condition using the *OrAny* method staring with an Or expression
* **IfAll** - If the condition is true, adds the condition using the *All* method
* **IfOrAll** - If the condition is true, adds the condition using the *OrAll* method staring with an Or expression
* **IfManyAny** - If the condition is true, adds the condition using the *ManyAny* method
* **IfOrManyAny** - If the condition is true, adds the condition using the *OrManyAny* method staring with an Or expression

### Sitecore Version
Tested on Sitecore 8.2+

### License
MIT License

Copyright (c) 2017 Kyle Kingsbury

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
