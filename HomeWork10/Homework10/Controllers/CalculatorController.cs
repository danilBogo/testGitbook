using Homework10.Models;
using Homework10.Services;
using Homework10.Services.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Homework10.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculator _calculator;

        public CalculatorController(ICalculator calculator)
        {
            _calculator = calculator;
        }
        
        [HttpGet]
        public IActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateMathExpression(string expression)
        {
            var result = _calculator.Calculate(expression);
            var model = result.Type is TypeResult.Success 
                ? new ResultModel($"Result: {result.Success}") 
                : new ResultModel($"Error: {result.Error}");
            return View("~/Views/Calculator/Calculator.cshtml", model);
        }
    }
}