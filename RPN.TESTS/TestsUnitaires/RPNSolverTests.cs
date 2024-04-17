using RPN.BLL.Interfaces;
using RPN.BLL.Services;

namespace RPN.TESTS.TestsUnitaires
{
    public class RPNSolverTests
    {
        private readonly IInputValidator _inputValidator;
        private readonly IRPNCalculator _rpnCalculator;

        public RPNSolverTests()
        {
            _inputValidator = new InputValidatorService();
            _rpnCalculator = new RPNCalculatorService();
        }

        [TestCase("3 3 +", true)]
        [TestCase("2 6 *", true)]
        [TestCase("20 6 /", true)]
        [TestCase("4 2 /", true)]
        [TestCase("3 5 / 8 + 2 ^ 5 -", true)]
        public void InputValidator_WithValidInput_ShouldReturnTrue(string input, bool resultExpected)
        {
            bool result = _inputValidator.IsValidRPN(input);
            Assert.That(resultExpected, Is.EqualTo(result));
        }

        [TestCase("3 3 +", 6)]
        [TestCase("2 6 *", 12)]
        [TestCase("20 6 /", 3.3333333333333335)]
        [TestCase("4 2 /", 2)]
        [TestCase("3 5 / 8 + 2 ^ 5 -", 68.96)]
        public void RPNSolver_WithValidInput_ShouldReturnCorrectResult(string input, double returnExpected)
        {
            double result = _rpnCalculator.RPNSolver(input);
            Assert.That(returnExpected, Is.EqualTo(result));
        }
    }
}