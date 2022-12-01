using TaxCalculator.Core.Models;

namespace TaxCalculator.Core.Domain.Interfaces
{
    public interface ITaxCalculator
    {
        bool IsNotTaxable(decimal grossIncome, out TaxCalculationResult? taxCalculationResult);
        decimal CalculateCharityAmount(decimal grossIncome, decimal charitySpent);
        decimal CalculateTaxableGrossIncome(decimal grossIncome, decimal charitySpent);
        decimal CalculateIncomeTax(decimal grossIncome);
        decimal CalculateSocialTax(decimal grossIncome);
    }
}
