using RPN.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN.BLL.Services
{
    public class RPNCalculatorService : IRPNCalculator
    {

        public double RPNSolver(string expression)
        {
            string[] tokens = expression.Split(' ');
            Stack<double> stack = new Stack<double>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();
                    double result = 0;

                    switch (token)
                    {
                        case "+":
                            result = operand1 + operand2;
                            break;
                        case "-":
                            result = operand1 - operand2;
                            break;
                        case "*":
                            result = operand1 * operand2;
                            break;
                        case "/":
                            if (operand2 != 0)
                                result = operand1 / operand2;
                            else
                                throw new DivideByZeroException("Erreur : Division par 0");
                            break;
                        case "^":
                            result = Math.Pow(operand1, operand2);
                            break;
                        default:
                            throw new ArgumentException("Erreur : le token ne correspond pas a un operateur => " + token);
                    }

                    stack.Push(result);
                }
            }

            if (stack.Count != 1)
            {
                throw new ArgumentException("expression invalide");
            }

            return stack.Pop();
        }
    }
}