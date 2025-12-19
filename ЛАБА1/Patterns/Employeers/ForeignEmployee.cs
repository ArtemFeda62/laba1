using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Employeers
{
    public class ForeignEmployee: Employee
    {
        public override string GetInfo()
        {
            return "Иностраный работник";
        }
    }
}
