using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN.BLL.Interfaces
{
    public interface IRPNCalculator
    {
        public double RPNSolver(string expression);
    }
}
