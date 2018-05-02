using System;
using System.Linq;
using Calculator.Contracts;
using Calculator.Models;

namespace Calculator.Services
{
    public class MathService : IMathService
    {
        public EquationModel BuildEquation(EquationModel equation, string input)
        {
            if (equation == null)
            {
                equation = new EquationModel();
            }

            if (input == "ce")
            {
                return new EquationModel();
            }

            if (input == "c")
            {
                if (string.IsNullOrWhiteSpace(equation.Operator))
                {
                    equation.Source1 = "";
                }
                else
                {
                    equation.Source2 = "";
                }

                return equation;
            }

            var operators = new[] {"+", "-", "x", "/"};
            if (operators.Contains(input))
            {
                equation.Operator = input;
            }
            else
            {
                // the first argument
                if (string.IsNullOrWhiteSpace(equation.Operator))
                {
                    equation.Source1 = AddInput(equation.Source1, input);
                }
                else
                {
                    equation.Source2 = AddInput(equation.Source2, input);
                }
            }

            return equation;
        }

        public decimal Calculate(EquationModel equation)
        {
            if (string.IsNullOrWhiteSpace(equation.Operator))
            {
                return 0;
            }

            switch (equation.Operator.ToLower())
            {
                case "+":
                    return equation.Argument1 + equation.Argument2;
                case "-":
                    return equation.Argument1 - equation.Argument2;
                case "x":
                    return equation.Argument1 * equation.Argument2;
                case "/":
                    return decimal.Divide(equation.Argument1, equation.Argument2);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        private string AddInput(string source, string input)
        {
            if (input == "." && source.Contains("."))
            {
                return source;
            }

            return source + input;
        }
    }
}