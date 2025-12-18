using Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employeers
{
    public abstract class Employee
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public ISalaryStrategy SalaryStrategy { get; set; }

        public abstract string GetInfo();

        public decimal GetSalaryAfterCommission()
        {
            if (SalaryStrategy == null)
                return Salary;
            return SalaryStrategy.CalculateFinalSalary(Salary);
        }
    }
}
