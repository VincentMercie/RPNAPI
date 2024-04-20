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
        /// Endpoint permettant de résoudre une expression au format RPN (Notation Polonaise Inversée) et de renvoyer le résultat du calcul.
        /// </summary>
        /// <remarks>
        /// Ce endpoint prend une expression RPN en entrée dans l'URL et tente de la résoudre. En cas de succès, le résultat du calcul est renvoyé. En cas d'erreur,
        /// des réponses appropriées sont renvoyées pour indiquer le problème rencontré.
        /// </remarks>
        /// <param name="expression">Expression RPN à évaluer.</param>
        /// <response code="200">Renvoyé si l'expression est évaluée avec succès. Le résultat du calcul est renvoyé dans le corps de la réponse.</response>
        /// <response code="400">Renvoyé si l'expression n'est pas au format RPN valide.</response>
        /// <response code="500">Renvoyé en cas d'erreur interne du serveur.</response>
        [HttpPost]
        [Route("{expression}")]
        public IActionResult Post(string expression)
        {
            try
            {
                //Simple nettoyage de l'expression passée en paramètre.
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
