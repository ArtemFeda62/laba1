using Employeers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class EnglishDecorator : EmployeeDecorator
    {
        public string CertificateName { get; set; }
        public DateTime CertificateDate { get; set; }

        public EnglishDecorator(Employee employee, string certificateName, DateTime certificateDate)
            : base(employee)
        {
            CertificateName = certificateName;
            CertificateDate = certificateDate;
        }

        public override string GetInfo()
        {
            return _employee.GetInfo() + $", Английский: Intermediate (Сертификат: {CertificateName}, {CertificateDate.ToShortDateString()})";
        }
    }
}
