// MIT License
// 
// Copyright (c) 2016 Kyle Kingsbury
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace Sitecore.ContentSearch.Fluent.Builders
{
    using Options;
    using Results;

    /// <summary>
    /// SearcherOptionsBuilder configures the Search Options
    /// </summary>
    public class PagingOptionsBuilder<T> where T : SearchResultItem
    {
        /// <summary>
        /// Builds the SearcherOptions
        /// </summary>
        protected readonly PagingOptions PagingOptions;

        public PagingOptionsBuilder(PagingOptions searchOptions)
        {
            this.PagingOptions = searchOptions;
        }

        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="pageMode">pageMode of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetPageMode(PageMode pageMode)
        {
            this.PagingOptions.PageMode = pageMode;
            return this;
        }

        /// <summary>
        /// Set the returned results page
        /// </summary>
        /// <param name="page">Page of the results</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetPage(int page)
        {
            this.PagingOptions.Start = page <= 0 ? 1 : page;
            return this;
        }

        /// <summary>
        /// Convenience method, calls <see cref="Take"/> internally
        /// </summary>
        /// <param name="size">Display Size</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetDisplaySize(int size)
        {
            return this.Take(size);
        }

        /// <summary>
        /// Convenience method, calls <see cref="Skip"/> internally
        /// <para>PASSES <c>True</c> to <see cref="Skip"/></para>
        /// </summary>
        /// <param name="start">Display Size</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> SetStartingPosition(int start)
        {
            return this.Skip(start, true);
        }

        /// <summary>
        /// Set the returned results page
        /// <para>Must set the PageMode to Start, otherwise sets the Paging inaccurately</para>
        /// </summary>
        /// <param name="start">Page of the results</param>
        /// <param name="includeStart">Should the results include the starting number</param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> Skip(int start, bool includeStart = false)
        {
            var temp = start;

            // Ensure we are working with a valid number
            if (temp < 0)
                temp = 0;

            this.PagingOptions.Start = (temp > 0) && includeStart
                ? temp - 1
                : temp;

            return this;
        }

        /// <summary>
        /// Sets the Display Size of the Returned Results
        /// </summary>
        /// <param name="display"></param>
        /// <returns>Instance of the SearcherOptionsBuilder</returns>
        public PagingOptionsBuilder<T> Take(int display)
        {
            this.PagingOptions.Display = display;
            return this;
        }
    }
}