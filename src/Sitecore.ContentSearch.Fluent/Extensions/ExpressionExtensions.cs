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
namespace Sitecore.ContentSearch.Fluent.Extensions
{
    using System;
    using System.Linq.Expressions;
    using Expressions;

    public static class ExpressionExtensions
    {
        /// <summary>
        /// Rewrites a given expression using the <see cref="ParameterReplaceVisitor"/> Expression Visitor
        /// by replacing the second argument of the initial expression with a constant value
        /// </summary>
        /// <param name="expression">Original Expression</param>
        /// <param name="value">Value to replace the 2nd parameter with</param>
        /// <returns>Rewritten Expression</returns>
        public static Expression<Func<T, bool>> Rewrite<T, TR>(this Expression<Func<T, TR, bool>> expression, TR value)
        {
            return
                Expression.Lambda<Func<T, bool>>(
                    new ParameterReplaceVisitor(expression.Parameters[1], value).Visit(expression.Body), expression.Parameters[0]);
        }
    }
}