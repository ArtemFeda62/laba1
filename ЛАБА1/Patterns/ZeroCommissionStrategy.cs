using Patterns;

public class ZeroCommissionStrategy : ISalaryStrategy
{
    public decimal CalculateFinalSalary(decimal salary)
    {
        return salary;
    }
}