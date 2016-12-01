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
