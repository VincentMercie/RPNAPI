using RPN.BLL.Interfaces;

namespace RPN.BLL.Services
{
    public class InputValidatorService : IInputValidator
    {
        public enum Operators
        {
            Addition = '+',
            Subtraction = '-',
            Multiplication = '*',
            Division = '/',
            Exponentiation = '^'
        }

        //Cette fonction va parcourir l'expression comme si elle voulait résoudre le calcul et vérifier si elle correspond à une notation RPN
        public bool IsValidRPN(string expression)
        {
            Stack<int> stack = new Stack<int>();

            // Divise l'expression en tokens
            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                // Vérifie si le token est une valeur numérique
                if (IsNumeric(token))
                {
                    // Si c'est le cas, empile un zéro sur la pile (on utilise 0 comme placeholder du resultat de chaque calcul)
                    stack.Push(0);
                }
                // Vérifie si le token est un opérateur
                else if (IsOperator(token))
                {
                    // Si la pile contient moins de deux valeurs (nécessaires pour l'opération), l'expression est invalide
                    if (stack.Count < 2)
                        return false;

                    // Retire deux valeurs de la pile pour les utiliser dans l'opération
                    stack.Pop();
                    stack.Pop();
                    // Empile un zéro comme résultat temporaire de l'opération
                    stack.Push(0);
                }
                else
                {
                    // Si le token n'est ni un nombre ni un opérateur, l'expression est invalide
                    return false;
                }
            }

            // Si à la fin du parcours il reste exactement une valeur sur la pile, l'expression est valide, sinon elle est invalide
            return stack.Count == 1;
        }

        private bool IsNumeric(string token)
        {
            return double.TryParse(token, out _);
        }

        private bool IsOperator(string token)
        {
            char opChar;
            if (token.Length == 1 && char.TryParse(token, out opChar))
            {
                return Enum.IsDefined(typeof(Operators), (int)opChar);
            }
            return false;
        }
    }
}