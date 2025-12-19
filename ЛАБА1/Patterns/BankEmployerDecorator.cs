using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class BankEmployeeDecorator : EmployeeDecorator
    {
        public string BankName { get; set; }
        public string EmployeeId { get; set; }
        public string Department { get; set; }

        public BankEmployeeDecorator(Employee employee, string bankName, string employeeId, string department)
            : base(employee)
        {
            BankName = bankName;
            EmployeeId = employeeId;
            Department = department;

            SalaryStrategy = new ZeroCommissionStrategy();
        }

        public override string GetInfo()
        {
            return _employee.GetInfo() + $", Сотрудник банка: {BankName} (ID: {EmployeeId}, Отдел: {Department})";
        }
    }
}