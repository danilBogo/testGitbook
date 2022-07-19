﻿using Homework10.DbModels;
using Homework10.Services.Calculator;

namespace Homework10.Services.HashedCalculator
{
	public class HashedCalculator : ICalculator
	{
		private readonly ApplicationContext _hashedExpression;
		private readonly ICalculator _calculator;

		public HashedCalculator(ApplicationContext hashedExpression, ICalculator calculator)
		{
			_hashedExpression = hashedExpression;
			_calculator = calculator;
		}

		public Result<string, string> Calculate(string expression)
		{
			var expressionWithoutSpace = expression?.Replace(" ", "");
			var possibleResult = _hashedExpression.SolvingExpressions
				.FirstOrDefault(exp => exp.Expression == expressionWithoutSpace)?.Result;
			if (possibleResult is not null)
				return new Result<string, string>(success: possibleResult);

			var result = _calculator.Calculate(expression);
			if (result.Type == TypeResult.Error)
				return result;

			var solvingExpression = new SolvingExpression {Expression = expressionWithoutSpace, Result = result.Success};
			_hashedExpression.Add(solvingExpression);
			_hashedExpression.SaveChanges();
			return result;
		}
	}
}