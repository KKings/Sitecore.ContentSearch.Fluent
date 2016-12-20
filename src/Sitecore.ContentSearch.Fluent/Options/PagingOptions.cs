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
namespace Sitecore.ContentSearch.Fluent.Options
{
    /// <summary>
    /// SearcherOptions Summary
    /// </summary>
    public class PagingOptions
    {
        /// <summary>
        /// Gets or sets the PageMode
        /// </summary>
        public virtual PageMode PageMode { get; set; } = PageMode.Pager;

        /// <summary>
        /// Gets or sets the Start
        /// </summary>
        public virtual int Start { get; set; } = 0;

        /// <summary>
        /// Gets or sets the returned results
        /// </summary>
        public virtual int Display { get; set; } = 10;

        /// <summary>
        /// Gets the calculated StartingPosition
        /// </summary>
        public virtual int StartingPosition
        {
            get
            {
                // If the PageMode is the Pager, we need to calculate the starting position
                if (this.PageMode == PageMode.Pager)
                {
                    return this.Start <= 1
                        ? 0
                        : (this.Start - 1) * this.Display;
                }

                return this.Start;
            }
        }
    }
}