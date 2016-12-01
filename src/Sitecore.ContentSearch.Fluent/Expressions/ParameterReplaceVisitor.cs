// <copyright file="Searcher.cs" company="Kyle Kingsbury">
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
namespace Sitecore.ContentSearch.Fluent.Expressions
{
    using System.Linq.Expressions;

    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Parameter Expression to be replaced with the Constant
        /// </summary>
        private readonly ParameterExpression parameterExpression;

        /// <summary>
        /// Constant Expression that will replace the Parameter Expression
        /// </summary>
        private readonly ConstantExpression constantExpression;

        public ParameterReplaceVisitor(ParameterExpression expression, object value)
        {
            this.parameterExpression = expression;
            this.constantExpression = Expression.Constant(value);
        }

        /// <summary>
        /// Replaces the Parameter Expression with a constant expression
        /// </summary>
        /// <param name="expression">Parameters within the Expression</param>
        /// <returns><c>The Constant Expression</c> if the parameter matches the designated expression</returns>
        protected override Expression VisitParameter(ParameterExpression expression)
        {
            if (expression == this.parameterExpression)
            {
                return this.constantExpression;
            }

            return expression;
        }
    }
}
