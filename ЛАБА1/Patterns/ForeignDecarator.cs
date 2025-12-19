using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class ForeignDecarator: EmployeeDecorator
    {
        Employee _employee;
        public bool Russian;
        public ForeignDecarator(Employee employee,bool russian)
            : base(employee)
        {
            employee = _employee;
            Russian = russian;
        }
        public override string GetInfo()
        {
            return _employee.GetInfo() + $" Иностранный работник:{Russian}";
        }
    }
}
