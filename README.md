# Fluent API for Sitecore.ContentSearch

A lightweight, fluent API that reduces the verbosity of the Sitecore.ContentSearch linq layer.

Features fluent API for:
- Sorting
    - Multile Sorting based on expressions
- Querying
    - Supports nested 'And' and 'Or' operations
- Filtering
    - Supports nested 'And' and 'Or' operations
- Pagination
- Template Restrictions

### Examples
##### Basic (Content Search vs Fluent API)
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

// Using the Fluent API
using (var manager = new SearchManager("sitecore_master_index", "sitecore_web_index"))
{
    var results = manager.ResultsFor<SearchResultItem>(search => search
        .Options(o => o
            .SetDisplaySize(15)
            .AddRestriction(ArticlePage.TemplateId))
        .Query(q => q
            .And(and => and
                .Where(result => result.Name.Like("test"))))
        .Sort(sort => sort
            .By(result => result.CreatedDate)));
}
```
###Sitecore Version and Search
Tested on Sitecore 7+ with Lucene. Fixes a Sitecore 7.2 bug.

###License
Copyright © 2015 Kyle Kingsbury

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an 'AS IS' BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.