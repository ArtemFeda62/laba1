using Employeers;
using Patterns;
using Patterns.Employeers;

class Program
{
    static void Main()
    {
        Employee emp = new Researcher { Name = "Иван Петров", Salary = 50000 };
        emp = new EnglishDecorator(emp, "TOEFL", new DateTime(2023, 5, 15));
        emp = new DegreeDecorator(emp, "Информатика", "AI in IoT", 2022);
        Console.WriteLine(emp.GetInfo());
        emp.SalaryStrategy = new SberbankStrategy();
        Console.WriteLine($"Зарплата через Сбербанк: {emp.GetSalaryAfterCommission()}");
        emp.SalaryStrategy = new GazprombankStrategy();
        Console.WriteLine($"Зарплата через Газпромбанк: {emp.GetSalaryAfterCommission()}");

        Employee emp1 = new ForeignEmployee { Name = "Jack Black", Salary = 100000 };
        emp1 = new ForeignDecarator ( emp1, false );
        Console.WriteLine(emp1.GetInfo());
        emp1.SalaryStrategy = new ForeignSrtategy();
    }
}