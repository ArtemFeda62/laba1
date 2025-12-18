using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Patterns
{
    public abstract class EmployeeDecorator : Employee
    {
        protected Employee _employee;

        public EmployeeDecorator(Employee employee)
        {
            _employee = employee;
            Name = employee.Name;
            Salary = employee.Salary;
        }

        public override abstract string GetInfo();
    }
}
