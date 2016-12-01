// <copyright file="FilterOptions.cs" company="Kyle Kingsbury">
//  Copyright 2015 Kyle Kingsbury
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.

//  You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an 'AS IS' BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// </copyright>
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