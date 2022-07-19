namespace Homework10.Services.Calculator
{
    public interface ICalculator
    {
        public Result<string, string> Calculate(string expression);
    }
}