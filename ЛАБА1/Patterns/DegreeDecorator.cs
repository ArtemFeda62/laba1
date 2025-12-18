using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class DegreeDecorator : EmployeeDecorator
    {
        public string ScienceField { get; set; }
        public string ThesisTopic { get; set; }
        public int DefenseYear { get; set; }

        public DegreeDecorator(Employee employee, string scienceField, string thesisTopic, int defenseYear)
            : base(employee)
        {
            ScienceField = scienceField;
            ThesisTopic = thesisTopic;
            DefenseYear = defenseYear;
        }

        public override string GetInfo()
        {
            return _employee.GetInfo() + $", Учёная степень: {ScienceField} (Тема: '{ThesisTopic}', {DefenseYear} г.)";
        }
    }
}
