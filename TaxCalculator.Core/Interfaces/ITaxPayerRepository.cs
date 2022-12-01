using TaxCalculator.Core.Models;

namespace TaxCalculator.Core.Interfaces
{
    public interface ITaxPayerRepository
    {
        Task<TaxCalculationResult?> Get(string ssn);
        Task<bool> Save(string ssn, TaxCalculationResult taxCalculationResult);
    }
}
