using Microsoft.AspNetCore.Mvc;
using RPN.BLL.Interfaces;
using RPN.BLL.Services;

namespace RPNAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RPNSolverController : ControllerBase
    {

        private readonly IInputValidator _inputValidator;
        private readonly IRPNCalculator _rpnCalculator;
        private readonly ILogger<RPNSolverController> _logger;

        public RPNSolverController(IInputValidator inputValidator, IRPNCalculator rpnCalculator, ILogger<RPNSolverController> logger)
        {
            _inputValidator = inputValidator;
            _rpnCalculator = rpnCalculator;
            _logger = logger;
        }
       
        [HttpPost]
        [Route("{expression}")]
        public IActionResult Post(string expression)
        {
            try
            {
                double result;
                if (_inputValidator.IsValidRPN(expression))
                {
                     result = _rpnCalculator.RPNSolver(expression);
                    _logger.LogInformation("Expression {expression} evaluated successfully.", expression);
                }
                else
                {
                    _logger.LogWarning($@"Invalid RPN expression: {expression}");
                    return BadRequest($@"le format de l'expression {expression} ne correspond pas à une expression en notation polonaise inversée");
                }
                 return Ok(result.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the expression: {expression}", expression);
                return Problem(ex.Message);
            }
        }
    }
}
