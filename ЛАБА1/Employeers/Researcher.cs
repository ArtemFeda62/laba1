using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employeers
{
    public class Researcher : Employee
    {
        public override string GetInfo()
        {
            return "Должность: Исследователь";
        }
    }
}
