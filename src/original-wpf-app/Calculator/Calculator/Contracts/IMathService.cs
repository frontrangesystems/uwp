using Calculator.Models;

namespace Calculator.Contracts
{
    public interface IMathService
    {
        EquationModel BuildEquation(EquationModel equation, string input);
        decimal Calculate(EquationModel equation);
    }
}