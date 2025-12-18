using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class SberbankStrategy : ISalaryStrategy
    {
        public decimal CalculateFinalSalary(decimal salary)
        {
            return salary * 0.99m; 
        }
    }

}
