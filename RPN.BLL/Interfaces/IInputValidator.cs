using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN.BLL.Interfaces
{
    public interface IInputValidator
    {
        public bool IsValidRPN(string expression);
    }
}
