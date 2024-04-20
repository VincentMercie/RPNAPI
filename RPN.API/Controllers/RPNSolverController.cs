using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Console;
using RPN.BLL.Interfaces;
using RPN.BLL.Services;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Endpoint permettant de r�soudre une expression au format RPN (Notation Polonaise Invers�e) et de renvoyer le r�sultat du calcul.
        /// </summary>
        /// <remarks>
        /// Ce endpoint prend une expression RPN en entr�e dans l'URL et tente de la r�soudre. En cas de succ�s, le r�sultat du calcul est renvoy�. En cas d'erreur,
        /// des r�ponses appropri�es sont renvoy�es pour indiquer le probl�me rencontr�.
        /// </remarks>
        /// <param name="expression">Expression RPN � �valuer.</param>
        /// <response code="200">Renvoy� si l'expression est �valu�e avec succ�s. Le r�sultat du calcul est renvoy� dans le corps de la r�ponse.</response>
        /// <response code="400">Renvoy� si l'expression n'est pas au format RPN valide.</response>
        /// <response code="500">Renvoy� en cas d'erreur interne du serveur.</response>
        [HttpPost]
        [Route("{expression}")]
        public IActionResult Post(string expression)
        {
            try
            {
                //Simple nettoyage de l'expression pass�e en param�tre.
                expression = Regex.Replace(expression.Replace('.', ','), @"\s+", " ");

                double result;
                if (_inputValidator.IsValidRPN(expression))
                {
                    result = _rpnCalculator.RPNSolver(expression);
                    _logger.LogInformation("Expression {expression} evaluated successfully.", expression);
                }
                else
                {
                    _logger.LogWarning($@"Invalid RPN expression: {expression}");
                    return BadRequest($@"le format de l'expression {expression} ne correspond pas � une expression en notation polonaise invers�e");
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
